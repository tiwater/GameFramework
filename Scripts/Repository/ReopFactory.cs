using System;
using System.Collections.Generic;

namespace GameFramework.Repository
{
    public class RepoFactory
    {
        public enum RepoMode { Pref, Rpc, Mock };
        protected RepoMode repoMode = RepoMode.Mock;
        protected static RepoMode defaultMode = RepoMode.Rpc;

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

        protected static RepoFactory _factoryInstance;

        protected Dictionary<Type, IRepository> repos = new Dictionary<Type, IRepository>();

        protected RepoFactory(RepoMode repoMode)
        {
            this.repoMode = repoMode;
        }

        /// <summary>
        /// Get the repository instance in the specified type
        /// </summary>
        /// <typeparam name="T">The interface type we expect</typeparam>
        /// <returns></returns>
        public static T GetRepository<T>() where T : IRepository
        {
            return (T)FactoryInstance.GetRepository(typeof(T));

        }

        /// <summary>
        /// Get repository by its type
        /// </summary>
        /// <param name="type">The repository's interface</param>
        /// <returns></returns>
        protected virtual IRepository GetRepository(Type type)
        {
            IRepository repository;
            //Get repository
            repos.TryGetValue(type, out repository);
            if (repository == null)
            {
                //If don't have instance yet, create one
                if (type == typeof(IPlayerGameItemRepository))
                {
                    if (repoMode == RepoMode.Pref)
                    {
                        //Prefs repositories
                        repository = new PlayerGameItemPrefRepository();
                    }
                    else if (repoMode == RepoMode.Rpc)
                    {
                        //Rpc repositories
                        repository = new PlayerGameItemRpcRepository();
                    }
                    else if (repoMode == RepoMode.Mock)
                    {
                        //Mock repositories
                        repository = new PlayerGameItemMockRepository();
                    }
                } else if (type == typeof(IOperationRepository))
                {
                    if (repoMode == RepoMode.Pref)
                    {
                        //Prefs repositories
                        throw new NotImplementedException();
                    }
                    else if (repoMode == RepoMode.Rpc)
                    {
                        //Rpc repositories
                        repository = new OperationRpcRepository();
                    }
                    else if (repoMode == RepoMode.Mock)
                    {
                        //Mock repositories
                        repository = new OperationMockRepository();
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

        /// <summary>
        /// Register a customized repository in the factory for a specific type
        /// </summary>
        /// <typeparam name="T">The repository interface we will register</typeparam>
        /// <param name="repository">The repository instance to register</param>
        public void RegisterRepository<T>(IRepository repository) where T : IRepository
        {
            repos.Add(typeof(T), repository);
        }
    }
}