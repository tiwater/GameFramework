using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameFramework.Repository
{
    public interface IPlayerGameItemRepository
    {
        Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item);
        Task UpdateParentChildRelation(string parentId, string childId, bool add);
        Task<string> LoadToken();
        Task StoreToken(string token);
        Task<PlayerGameItem> GetPlayerGameItem(string itemId);
        /// <summary>
        /// Load the children PlayerGameItem under the given itemId
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId);
    }
}