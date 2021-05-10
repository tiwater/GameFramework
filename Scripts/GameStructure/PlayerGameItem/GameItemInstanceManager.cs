using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.Service;
using UnityEngine;

namespace GameFramework.GameStructure.PlayerGameItems
{
    /// <summary>
    /// The class to manage the PlayGameItem, which is instance of GameItem
    /// </summary>
    public class GameItemInstanceManager
    {
        /// <summary>
        /// The root PlayerGameItem
        /// </summary>
        public PlayerGameItem Root { get; set; }

        Dictionary<string, PlayerGameItem> Items = new Dictionary<string, PlayerGameItem>();

        /// <summary>
        /// Load the PlayerGameItem in current actived scene
        /// </summary>
        /// <returns></returns>
        public async Task LoadItemInCurrentScene()
        {
            Root = await PlayerGameItemService.Instance.LoadCurrentScene();
            await PopulateGameItem(Root);
        }

        /// <summary>
        /// Populate the assets of the GameItems
        /// </summary>
        /// <param name="pgi"></param>
        /// <returns></returns>
        private async Task PopulateGameItem(PlayerGameItem pgi)
        {
            //TODO: Optimize the process here. Start the async loading tasks together but not start one by one.
            await GameManager.Instance.GetIBaseGameItemManager(pgi.GiType).LoadAddressableResourceFromPlayerGameItem(pgi);
        }

        /// <summary>
        /// Get PlayerGameItem by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlayerGameItem GetPlayerGameItemById(string id)
        {
            PlayerGameItem item;
            Items.TryGetValue(id, out item);
            if (item != null)
            {
                return item;
            }
            item = SearchPlayerGameItem(id, Root);
            //TODO: Try to load from server
            if(item == null)
            {

            }
            Items[id] = item;
            return item;
        }

        private PlayerGameItem SearchPlayerGameItem(string id, PlayerGameItem root)
        {
            if (root.Id == id)
            {
                return root;
            }
            else
            {
                if (root.Children != null)
                {
                    //Search in children
                    foreach(var child in root.Children)
                    {
                        var item = SearchPlayerGameItem(id, child);
                        if (item != null)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}