using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;

namespace GameFramework.Repository
{
    public interface IPlayerGameItemRepository : IRepository
    {
        Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item);
        Task UpdateParentChildRelation(string parentId, string childId, bool add);
        Task<string> LoadToken();
        Task StoreToken(string token);

        /// <summary>
        /// Get the active player on the device
        /// </summary>
        /// <returns></returns>
        Task<PlayerGameItem> GetCurrentPlayerInstance();
        Task<PlayerGameItem> GetPlayerGameItem(string itemId);

        /// <summary>
        /// Load the children PlayerGameItem under the given itemId
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId);

        /// <summary>
        /// Load the scene the player located in
        /// </summary>
        /// <returns></returns>
        Task<PlayerGameItem> LoadCurrentScene(string scene);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        Task<List<PlayerGameItem>> GetEquipments(string itemId);
    }
}