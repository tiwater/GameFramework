using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.GameStructure.Model;
using GameFramework.GameStructure.Util;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Service
{
    public class GameItemMetaService
    {
        //The callback when get the available GameItem list
        public delegate void GameItemMetasHandler(AddressableGameItemMeta[] gameItems);

        public static GameItemMetaService Instance = new GameItemMetaService();

        /// <summary>
        /// Refresh the meta data from server
        /// </summary>
        /// <param name="type"></param>
        /// <param name="GiId"></param>
        /// <returns></returns>
        public async Task PopulateGameItem(GameItem item)
        {
            string strMeta = PlayerPrefs.GetString(item.GetType().Name.ToString() + item.GiId);
            JsonUtility.FromJsonOverwrite(strMeta, item);

            return;
        }


        public async Task UpdateGameItem(GameItem item)
        {
            string json = JsonUtility.ToJson(item);
            PlayerPrefs.SetString(item.GetType().Name.ToString() + item.GiId, json);

            return;
        }

        ///// <summary>
        ///// Get the available GameItemPOs for current user
        ///// </summary>
        ///// <param name="handler"></param>
        ///// <returns></returns>
        //public IEnumerator GetAvailableItemMetas(GameItemMetasHandler handler)
        //{
        //    //yield return (HttpUtil.Get(GlobalConstants.SERVER_TISVC_PREFIX + "/{AppId}/AvailableGameItemMeta", webRequest =>
        //    //{
        //    //    GameItemMeta[] itemMetas = JsonUtility.FromJson<GameItemMeta[]>(webRequest.downloadHandler.text);
        //    //    //GameItemMeta[] itemMetas = JsonMapper.ToObject<GameItemMeta[]>(webRequest.downloadHandler.text);

        //    yield return (HttpUtil.GetDummy(GlobalConstants.SERVER_TISVC_PREFIX + "/{AppId}/AvailableGameItemMeta", webRequest =>
        //    {
        //        AddressableGameItemMeta[] itemMetas = MockItems();
        //        if (handler != null)
        //        {
        //            handler(itemMetas);
        //        }

        //    }));
        //}

        //private List<GameItemMeta> MockItems()
        //{
        //    List<GameItemMeta> gameItemMetas = new List<GameItemMeta>();
        //    var loadedItem = GameItem.LoadFromResources<Level>("Level", "sea1");
        //    int i = 0;
        //    int initCount = 5;
        //    AddressableGameItemMeta[] itemMetas = new AddressableGameItemMeta[initCount];
        //    for (; i < initCount; i++)
        //    {
        //        itemMetas[i] = new AddressableGameItemMeta();
        //        itemMetas[i].Id = (i + 1).ToString();

        //        string fishSkinLabel = "Fish_skin" + (i + 1);
        //        itemMetas[i].Name = "Colorful fish skin";
        //        itemMetas[i].Description = itemMetas[i].Name;
        //        itemMetas[i].AppIds = new string[1];
        //        itemMetas[i].AppIds[0] = "1";
        //        itemMetas[i].Package = "Fish_skin" + (i + 1);
        //        itemMetas[i].Type = AddressableGameItemMeta.ContentType.Wearables;
        //        itemMetas[i].Consumable = false;
        //        itemMetas[i].Resources = new Dictionary<string, string>();
        //        //itemMetas[i].Resources.Add("眼珠", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼鳍1", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼鳍2", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼鳍3", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼身体", fishSkinLabel);
        //    }

        //    itemMetas[2].Type = AddressableGameItemMeta.ContentType.Entity;

        //    return gameItemMetas;
        //}

        //private AddressableGameItemMeta[] MockItems()
        //{

        //    int i = 0;
        //    int initCount = 5;
        //    AddressableGameItemMeta[] itemMetas = new AddressableGameItemMeta[initCount];
        //    for (; i < initCount; i++)
        //    {
        //        itemMetas[i] = new AddressableGameItemMeta();
        //        itemMetas[i].Id = (i + 1).ToString();

        //        string fishSkinLabel = "Fish_skin" + (i + 1);
        //        itemMetas[i].Name = "Colorful fish skin";
        //        itemMetas[i].Description = itemMetas[i].Name;
        //        itemMetas[i].AppIds = new string[1];
        //        itemMetas[i].AppIds[0] = "1";
        //        itemMetas[i].Package = "Fish_skin" + (i + 1);
        //        itemMetas[i].Type = AddressableGameItemMeta.ContentType.Wearables;
        //        itemMetas[i].Consumable = false;
        //        itemMetas[i].Resources = new Dictionary<string, string>();
        //        //itemMetas[i].Resources.Add("眼珠", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼鳍1", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼鳍2", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼鳍3", fishSkinLabel);
        //        //itemMetas[i].Resources.Add("鱼身体", fishSkinLabel);
        //    }

        //    itemMetas[2].Type = AddressableGameItemMeta.ContentType.Entity;

        //    return itemMetas;
        //}


        /// <summary>
        /// Upload the GameItem meta info to server for server side management and web presentation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameItems">The GameItem definitions load from the AssetDatabase</param>
        public void UploadGameItemMetas<T>(List<T> gameItems) where T : GameItem
        {
            string gameItemsJsonStr;
            JArray gameItemsJsonArray = new JArray();
            foreach (var gameItem in gameItems)
            {
                string name = gameItem.name;
                string GiId = name.Substring(name.IndexOf('_') + 1);
                string itemStr = JsonUtil.ExportGameItemMeta(gameItem);
                Assert.AreEqual(name, gameItem.IdentifierBase + "_" + GiId, string.Format("The name of resource {0} is not proper, {1} is expected.",
                    name, gameItem.IdentifierBase + "_" + GiId));
                JObject itemNode = JObject.Parse(itemStr);
                //Append item type
                itemNode.Add(new JProperty("GiType", gameItem.GetType().Name));
                gameItemsJsonArray.Add(itemNode);
            }
            gameItemsJsonStr = gameItemsJsonArray.ToString();
            //TODO: Call HTTPUtil to upload the data

        }
    }
}