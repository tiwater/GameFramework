using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;

namespace GameFramework.Repository
{
    public class PlayerGameItemRpcRepository : BaseRepository, IPlayerGameItemRepository
    {
        public Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerGameItem> GetCurrentPlayerInstance()
        {
            //TODO: Get from RPC
            PlayerGameItem player = new PlayerGameItem();
            player.Id = GameManager.Instance.UserId;
            return player;
        }

        public Task<PlayerGameItem> GetPlayerGameItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<PlayerGameItem> LoadCurrentScene()
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