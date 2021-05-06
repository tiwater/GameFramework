using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameFramework.Repository
{
    public class PlayerGameItemRpcRepository : BaseRepository, IPlayerGameItemRepository
    {
        public Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            throw new NotImplementedException();
        }

        public Task<PlayerGameItem> GetPlayerGameItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoadToken()
        {
            throw new NotImplementedException();
        }

        public Task StoreToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task UpdateParentChildRelation(string parentId, string childId, bool add)
        {
            throw new NotImplementedException();
        }
    }
}