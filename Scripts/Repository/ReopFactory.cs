using System;
using System.Collections.Generic;

namespace GameFramework.Repository
{
    public class RepoFactory
    {
        private enum RepoMode { Pref, Rpc, Mock };
        private RepoMode repoMode = RepoMode.Mock;

        public static IPlayerGameItemRepository PlayerGameItemRepository
        {
            get
            {
                return FactoryInstance.GetRepository(typeof(IPlayerGameItemRepository)) as IPlayerGameItemRepository;
            }
        }

        private static RepoFactory FactoryInstance = new RepoFactory();

        private Dictionary<Type, BaseRepository> repos = new Dictionary<Type, BaseRepository>();

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
    }
}