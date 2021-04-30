using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Model;
using GameFramework.GameStructure.Util;
using UnityEngine;
using static GameFramework.GameStructure.Model.GameItemEquipment;

namespace GameFramework.GameStructure.Service
{
    public class PlayerGameItemService : BaseService
    {
        //The callback when get the ExtGameItem list
        public delegate void PlayerGameItemHandler(List<PlayerGameItem> gameItems);

        private static string MOCK_FISH1_ID = "f1";
        private static string MOCK_FISH2_ID = "f2";

        private static string MOCK_SKIN_ID1 = "sk1";
        private static string MOCK_SKIN_ID2 = "sk2";
        private static string MOCK_STICK_ID1 = "stick1";


        public static string DUMMY_TOKEN_KEY = "PlayerToken";
        public static string DUMMY_PREFS_KEY = "Overall_data";

        private static PlayerGameItemService _instance = new PlayerGameItemService();

        public static PlayerGameItemService Instance
        {
            get { return _instance; }
        }

        public string LoadToken()
        {
            //TODO: get token from system
            if (string.IsNullOrEmpty(Token))
            {
                _token = PlayerPrefs.GetString(DUMMY_TOKEN_KEY, null);
            }
            return Token;
        }

        public void StoreToken(string token)
        {
            PlayerPrefs.SetString(DUMMY_TOKEN_KEY, token);
        }

        public async Task<PlayerGameItem> GetPlayerInstance()
        {
            //TODO:Get player profile by token
            string token = LoadToken();
            string playerId = GameManager.Instance.UserId;

            PlayerGameItem player = await HttpUtil.GetDummyAsync<PlayerGameItem>(GlobalConstants.SERVER_TISVC_PREFIX + "/Player");
            string playerString = PlayerPrefs.GetString(DUMMY_PREFS_KEY + playerId);
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

        //public async Task<PlayerGameItem> GetPlayer()
        //{
        //    //TODO:Get player profile by token
        //    string token = LoadToken();
        //    string playerId = GameManager.Instance.UserId;

        //    PlayerGameItem player = await HttpUtil.GetDummyAsync<PlayerGameItem>(GlobalConstants.SERVER_TISVC_PREFIX + "/Player");
        //    string playerString = PlayerPrefs.GetString(DUMMY_PREFS_KEY + playerId);
        //    if (playerString == null || playerString == "")
        //    //if (true)
        //    {
        //        //Create the default one
        //        playerString = JsonUtility.ToJson(MockPlayerInstance());
        //        PlayerPrefs.SetString(DUMMY_PREFS_KEY + playerId, playerString);
        //    }
        //    player = JsonUtility.FromJson<PlayerGameItem>(playerString);

        //    return player;
        //}

        public PlayerGameItem MockPlayerInstance()
        {

            PlayerGameItem p = new PlayerGameItem();
            p.Id = GameManager.Instance.UserId;
            //p.OwnedItems = MockGameItems();
            //p.OwnedItems = MockItems();
            //p.CharacterEquipments = MockEquipments();

            return p;
        }

        public PlayerDto MockPlayer()
        {

            PlayerDto p = new PlayerDto();
            p.Id = GameManager.Instance.UserId;
            //p.OwnedItems = MockGameItems();
            p.OwnedItems = MockItems();
            //p.CharacterEquipments = MockEquipments();

            return p;
        }

        public void UpdatePlayer()
        {
            string playerString = JsonUtility.ToJson(GameManager.Instance.Players.Selected.PlayerDto);
            string playerId = GameManager.Instance.UserId;
            PlayerPrefs.SetString(DUMMY_PREFS_KEY + playerId, playerString);
        }


        /// <summary>
        /// Load the GameItems the player owns from the server
        /// </summary>
        /// <param name="handler"></param>
        public async Task<List<PlayerGameItem>> LoadPlayerGameItems(string playerId)
        {
            //yield return (HttpUtil.Get(GlobalConstants.SERVER_TISVC_PREFIX + "/{AppId}/GameItemMeta", webRequest =>
            //{
            //    GameItemMeta[] itemMetas = JsonUtility.FromJson<GameItemMeta[]>(webRequest.downloadHandler.text);
            //    //GameItemMeta[] itemMetas = JsonMapper.ToObject<GameItemMeta[]>(webRequest.downloadHandler.text);

            await (HttpUtil.GetDummyAsync(GlobalConstants.SERVER_TISVC_PREFIX + "/{AppId}/PlayerGameItem"));
            //TODO: Load form web then sync to local, or load from local if web is not available
            List<PlayerGameItem> items = MockItems();
            PopulatePlayerGameItems(playerId, items);
            return items;
        }

        //private Dictionary<string, List<CharacterEquipment>> MockEquipments()
        //{
        //    Dictionary<string, List<CharacterEquipment>> equipments = new Dictionary<string, List<CharacterEquipment>>();

        //    //The HTY skin
        //    List<CharacterEquipment> el1 = new List<CharacterEquipment>();

        //    CharacterEquipment ce1 = new CharacterEquipment(MOCK_FISH2_ID, Slot.Default, MOCK_SKIN_ID2);
        //    el1.Add(ce1);
        //    CharacterEquipment ce2 = new CharacterEquipment(MOCK_FISH2_ID, Slot.RHand, MOCK_STICK_ID1);
        //    el1.Add(ce2);

        //    equipments.Add(MOCK_FISH2_ID, el1);

        //    //Fish1 skin
        //    List<CharacterEquipment> el2 = new List<CharacterEquipment>();
        //    CharacterEquipment ce22 = new CharacterEquipment(MOCK_FISH1_ID, Slot.Default, MOCK_SKIN_ID1);
        //    el2.Add(ce22);

        //    equipments.Add(MOCK_FISH1_ID, el2);


        //    return equipments;
        //}

        private Dictionary<string, GameItemEquipment[]> MockEquipments()
        {
            Dictionary<string, GameItemEquipment[]> equipments = new Dictionary<string, GameItemEquipment[]>();

            //The HTY skin
            List<GameItemEquipment> el1 = new List<GameItemEquipment>();

            GameItemEquipment ce1 = new GameItemEquipment(MOCK_FISH2_ID, Slot.Root, MOCK_SKIN_ID2);
            el1.Add(ce1);
            GameItemEquipment ce2 = new GameItemEquipment(MOCK_FISH2_ID, Slot.RHand, MOCK_STICK_ID1);
            el1.Add(ce2);

            equipments.Add(MOCK_FISH2_ID, el1.ToArray());

            //Fish1 skin
            List<GameItemEquipment> el2 = new List<GameItemEquipment>();
            GameItemEquipment ce22 = new GameItemEquipment(MOCK_FISH1_ID, Slot.Root, MOCK_SKIN_ID1);
            el2.Add(ce22);

            equipments.Add(MOCK_FISH1_ID, el2.ToArray());


            return equipments;
        }

        private List<PlayerGameItem> MockItems()
        {

            var items = new List<PlayerGameItem>();

            PlayerGameItem pgi = new PlayerGameItem();
            pgi.GiType = "Character";
            pgi.GameItem_name = "Character_hty";
            pgi.Id = MOCK_FISH2_ID;
            //pgi.Position = new Vector3(0, 0 , 0);
            //pgi.Rotation = new Vector3(0, 0, 0);// Quaternion.identity.eulerAngles;
            pgi.PrefabType = GameItem.LocalisablePrefabType.Stage1;
            pgi.IsActive = true;
            items.Add(pgi);

            //The HTY skin
            List<GameItemEquipment> el1 = new List<GameItemEquipment>();

            GameItemEquipment ce1 = new GameItemEquipment(MOCK_FISH2_ID, Slot.Root, MOCK_SKIN_ID2);
            el1.Add(ce1);
            GameItemEquipment ce2 = new GameItemEquipment(MOCK_FISH2_ID, Slot.RHand, MOCK_STICK_ID1);
            el1.Add(ce2);

            pgi.CharacterEquipments = el1;

            PlayerGameItem pgi2 = new PlayerGameItem();
            pgi2.GiType = "Level";
            pgi2.GameItem_name = "Level_forest";
            pgi2.Id = "abcd";
            items.Add(pgi2);

            PlayerGameItem pgi3 = new PlayerGameItem();
            pgi3.GiType = "AddressableGameItem";
            pgi3.GameItem_name = "AGI_Fish1Skin1";
            pgi3.Id = MOCK_SKIN_ID1;
            items.Add(pgi3);

            PlayerGameItem pgi4 = new PlayerGameItem();
            pgi4.GiType = "Character";
            pgi4.GameItem_name = "Character_Fish1";
            pgi4.Id = MOCK_FISH1_ID;
            //pgi4.Position = new Vector3(0, 1, 0);
            //pgi4.Rotation = new Vector3(0, 0, 0);// Quaternion.identity.eulerAngles;
            pgi4.PrefabType = GameItem.LocalisablePrefabType.Stage1;
            pgi4.IsActive = true;
            items.Add(pgi4);


            //Fish1 skin
            List<GameItemEquipment> el2 = new List<GameItemEquipment>();
            GameItemEquipment ce22 = new GameItemEquipment(MOCK_FISH1_ID, Slot.Root, MOCK_SKIN_ID1);
            el2.Add(ce22);
            pgi4.CharacterEquipments = el2;

            PlayerGameItem pgi5 = new PlayerGameItem();
            pgi5.GiType = "AddressableGameItem";
            pgi5.GameItem_name = "AGI_Fish2Skin1";
            pgi5.Id = MOCK_SKIN_ID2;
            items.Add(pgi5);

            PlayerGameItem pgi6 = new PlayerGameItem();
            pgi6.GiType = "AddressableGameItem";
            pgi6.GameItem_name = "AGI_Stick1";
            pgi6.Id = MOCK_STICK_ID1;
            items.Add(pgi6);

            return items;
        }


        /// <summary>
        /// Set the GameItem status based on the PlayerGameItem
        /// </summary>
        /// <param name="playerGameItems"></param>
        /// <returns></returns>
        private void PopulatePlayerGameItems(string playerId, List<PlayerGameItem> playerGameItems)
        {
            foreach (PlayerGameItem playerGameItem in playerGameItems)
            {
                var gameItemManager = GameManager.Instance.GetIBaseGameItemManager(playerGameItem.GiType);
                GameItem addrItem = gameItemManager.BaseGetItem(playerGameItem.GiId);
                if (addrItem != null)
                {
                    addrItem.IsUnlocked = true;
                    //if (addrItem.GetType() == typeof(AddressableGameItem))
                    //{
                    //    //Addressable Game Items may have quantity
                    //    addrItem.GetCounter(Constants.QUANTITY_COUNTER).Set(playerGameItem.Amount);
                    //}
                }
                else
                {
                    //Has GameItem not fetched, need to fetch again later
                    Debug.LogWarning(string.Format("No GameItem available for {0}", playerGameItem.GameItem_name));
                }
            }
            if (GameManager.Instance.Players.Selected.GiId == playerId)
            {
                GameManager.Instance.Players.Selected.PlayerDto.OwnedItems = playerGameItems;
            }
            Debug.Log("PopulatePlayerGameItems Done");
        }

        public async Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            var playerString = JsonUtility.ToJson(item);
            PlayerPrefs.SetString(DUMMY_PREFS_KEY + item.Id, playerString);
            return JsonUtility.FromJson<PlayerGameItem>(playerString);
        }
    }
}