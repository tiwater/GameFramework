using System;
using System.Collections.Generic;

namespace GameFramework.Repository
{
    public class RepoFactory
    {
        public enum RepoMode { Pref, Rpc, Mock };
        private RepoMode repoMode = RepoMode.Mock;
        private static RepoMode defaultMode = RepoMode.Rpc;

        /// <summary>
        /// Specifiy the repository mode we want
        /// </summary>
        /// <param name="repoMode"></param>
        public static void InitFactoryInstance(RepoMode repoMode)
        {
            if (_factoryInstance == null)
            {
                //Generate default repo factory
                _factoryInstance = new RepoFactory(repoMode);
            }
        }

        /// <summary>
        /// Get the PlayerGameItemRepository 
        /// </summary>
        public static IPlayerGameItemRepository PlayerGameItemRepository
        {
            get
            {
                return FactoryInstance.GetRepository(typeof(IPlayerGameItemRepository)) as IPlayerGameItemRepository;
            }
        }

        /// <summary>
        /// The RepoFactory instance, which is reponsible for create the repositories
        /// </summary>
        public static RepoFactory FactoryInstance {
            get
            {
                if (_factoryInstance == null)
                {
                    //Generate default repo factory
                    _factoryInstance = new RepoFactory(defaultMode);
                }
                return _factoryInstance;
            }
        }

        private static RepoFactory _factoryInstance;

        private Dictionary<Type, BaseRepository> repos = new Dictionary<Type, BaseRepository>();

        private RepoFactory(RepoMode repoMode)
        {
            this.repoMode = repoMode;
        }

        /// <summary>
        /// Get repository by its type
        /// </summary>
        /// <param name="type">The repository's interface</param>
        /// <returns></returns>
        private BaseRepository GetRepository(Type type)
        {
            BaseRepository repository;
            //Get repository
            repos.TryGetValue(type, out repository);
            if (repository == null)
            {
                //If don't have instance yet, create one
                if (repoMode == RepoMode.Pref)
                {
                    //Prefs repositories
                    if (type == typeof(IPlayerGameItemRepository))
                    {
                        repository = new PlayerGameItemPrefRepository();
                    }
                }
                else if (repoMode == RepoMode.Rpc)
                {
                    //Rpc repositories
                    if (type == typeof(IPlayerGameItemRepository))
                    {
                        repository = new PlayerGameItemRpcRepository();
                    }
                }
                else if (repoMode == RepoMode.Mock)
                {
                    //Rpc repositories
                    if (type == typeof(IPlayerGameItemRepository))
                    {
                        repository = new PlayerGameItemMockRepository();
                    }
                }
                if (repository != null)
                {
                    //Save to dictionary
                    repos.Add(type, repository);
                }
            }
            return repository;
        }

        private void RegisterRepository(Type type, BaseRepository repository)
        {
            repos.Add(type, repository);
        }

        /// <summary>
        /// Register a repository for PlayerGameItemRepository
        /// </summary>
        /// <param name="repository"></param>
        public void RegisterPlayerGameItemRepository(BaseRepository repository)
        {
            RegisterRepository(typeof(IPlayerGameItemRepository), repository);
        }
    }
}