using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameFramework.GameStructure.Characters.ObjectModel;
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

        public PlayerGameItem SelectedCharacter { get; set; }

        Dictionary<string, PlayerGameItem> Items = new Dictionary<string, PlayerGameItem>();

        /// <summary>
        /// Load the PlayerGameItem in current actived scene
        /// </summary>
        /// <returns></returns>
        public async Task LoadItemInCurrentScene()
        {
            Root = await PlayerGameItemService.Instance.LoadCurrentScene(GameManager.Instance.GameName);
            await PopulateGameItem(Root);
        }

        /// <summary>
        /// Populate the assets of the GameItems
        /// </summary>
        /// <param name="pgi"></param>
        /// <returns></returns>
        public async Task PopulateGameItem(PlayerGameItem pgi)
        {
            Dictionary<string, List<string>> gameItems = new Dictionary<string, List<string>>();
            FetchGameItems(pgi, gameItems);
            //Load the GameItem assets.
            var resources = gameItems.Select(pair => GameManager.Instance.GetIBaseGameItemManager(pair.Key).LoadAddressableResources(pair.Value));
            await Task.WhenAll(resources);

        }

        /// <summary>
        /// Get the GameItems from the PlayerGameItem and its children, then we can preload the resources in GameItems
        /// </summary>
        /// <param name="pgi"></param>
        /// <param name="gameItems"></param>
        private void FetchGameItems(PlayerGameItem pgi, Dictionary<string, List<string>> gameItems)
        {
            List<string> items;

            if(pgi.GiType == typeof(Character).Name && pgi.IsActive)
            {
                SelectedCharacter = pgi;
            }

            gameItems.TryGetValue(pgi.GiType, out items);
            if(items == null)
            {
                items = new List<string>();
                items.Add(pgi.GiId);
                gameItems[pgi.GiType] = items;
            } else if (!items.Contains(pgi.GiId))
            {
                items.Add(pgi.GiId);
            }
            if (pgi.Children != null)
            {
                foreach(var child in pgi.Children)
                {
                    FetchGameItems(child, gameItems);
                }
            }


            if (pgi.Equipments != null)
            {
                //Load equipments
                foreach (var equipment in pgi.Equipments)
                {
                    FetchGameItems(equipment, gameItems);
                }
            }

            //if (pgi.CharacterEquipments != null)
            //{
            //    //Load equipments
            //    foreach (var equipment in pgi.CharacterEquipments)
            //    {
            //        var equipItem = GameManager.Instance.PlayerGameItems.GetPlayerGameItemById(equipment.GameItemId);
            //        if (equipItem != null)
            //        {
            //            FetchGameItems(equipItem, gameItems);
            //        }
            //    }
            //}
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