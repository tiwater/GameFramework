﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Util;
using UnityEngine;

namespace GameFramework.Repository
{
    public class PlayerGameItemPrefRepository : BaseRepository, IPlayerGameItemRepository
    {

        public static string PREFS_TOKEN_KEY = "PlayerToken";
        public static string PREFS_GAMEITEM_KEY = "GameItem";
        public static string PREFS_CHILDREN = "ChildrenId";

        public async Task UpdateParentChildRelation(string parentId, string childId, bool add)
        {
            var childrenId = PlayerPrefs.GetString(PREFS_CHILDREN + parentId);
            if (string.IsNullOrEmpty(childrenId))
            {
                if (add)
                {
                    childrenId = childId;
                }
            } else {
                //Convert string to List
                var childrenIdArray = childrenId.Split(',');
                List<string> childrenIdList = new List<string>(childrenIdArray);
                if (childrenIdList.Contains(childId))
                {
                    //If we owned the child already
                    if (!add)
                    {
                        //And request to remove
                        childrenIdList.Remove(childId);
                    }
                } else
                {
                    if (add)
                    {
                        //Add if we didn't own the child yet
                        childrenIdList.Add(childId);
                    }
                }
                var childrenSb = new StringBuilder();
                foreach (var child in childrenIdList)
                {
                    childrenSb.Append(child);
                    childrenSb.Append(",");
                }
                childrenSb.Length = childrenSb.Length - 1;
                childrenId = childrenSb.ToString();
            }
            PlayerPrefs.SetString(PREFS_CHILDREN + parentId, childrenId);
        }

        public async Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            var playerString = JsonUtility.ToJson(item);
            PlayerPrefs.SetString(PREFS_GAMEITEM_KEY + item.Id, playerString);
            return JsonUtility.FromJson<PlayerGameItem>(playerString);
        }
        public async Task<string> LoadToken()
        {
            //TODO: get token from system
            if (string.IsNullOrEmpty(Token))
            {
                _token = PlayerPrefs.GetString(PREFS_TOKEN_KEY, null);
            }
            return Token;
        }

        public async Task StoreToken(string token)
        {
            PlayerPrefs.SetString(PREFS_TOKEN_KEY, token);
        }

        public async Task<PlayerGameItem> GetPlayerGameItem(string itemId)
        {

            PlayerGameItem player = await HttpUtil.GetDummyAsync<PlayerGameItem>(GlobalConstants.SERVER_TISVC_PREFIX + "/Player");
            string playerString = PlayerPrefs.GetString(PREFS_GAMEITEM_KEY + itemId);
            if (string.IsNullOrEmpty(playerString))
            //if (true)
            {
                player = null;
                //Commented as we want to trigger the new account creation in client
                //Create the default one
                //playerString = JsonUtility.ToJson(MockPlayerInstance());
                //PlayerPrefs.SetString(DUMMY_PREFS_KEY + playerId, playerString);
            }
            player = JsonUtility.FromJson<PlayerGameItem>(playerString);

            return player;
        }

        public async Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId)
        {
            List<PlayerGameItem> items = new List<PlayerGameItem>();
            var childrenId = PlayerPrefs.GetString(PREFS_CHILDREN + itemId);
            if (!string.IsNullOrEmpty(childrenId))
            {
                var childrenIdArray = childrenId.Split(',');
                //Load all children
                var itemsSelector = childrenIdArray.Select(id => GetPlayerGameItem(id));
                await Task.WhenAll(itemsSelector);
                foreach(var item in itemsSelector)
                {
                    //Add to list
                    items.Add(await item);
                }
            }
            return items;
        }

        public Task<PlayerGameItem> GetCurrentPlayerInstance()
        {
            return GetPlayerGameItem(GameManager.Instance.UserId);
        }

        public Task<PlayerGameItem> LoadCurrentScene(string theme)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlayerGameItem>> GetEquipments(string itemId)
        {
            throw new NotImplementedException();
        }
    }
}