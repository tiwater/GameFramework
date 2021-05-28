using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Util;
using GameFramework.Repository;
using static GameFramework.GameStructure.PlayerGameItems.ObjectModel.GameItemEquipment;

namespace GameFramework.Service
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

        protected static PlayerGameItemService _instance;

        public static PlayerGameItemService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerGameItemService();
                }
                return _instance;
            }
        }

        public async Task<string> LoadToken()
        {
            return await RepoFactory.PlayerGameItemRepository.LoadToken();
        }

        public async Task StoreToken(string token)
        {
            await RepoFactory.PlayerGameItemRepository.StoreToken(token);
        }


        public async Task<PlayerGameItem> GetCurrentPlayerInstance()
        {

            return await RepoFactory.PlayerGameItemRepository.GetCurrentPlayerInstance();
        }

        public async Task<PlayerGameItem> GetPlayerInstance()
        {
            //TODO:Get player profile by token
            string token = await LoadToken();
            string playerId = GameManager.Instance.UserId;

            PlayerGameItem player = await GetPlayerGameItem(playerId);

            return player;
        }

        public async Task<PlayerGameItem> GetPlayerGameItem(string itemId)
        {
            return await RepoFactory.PlayerGameItemRepository.GetPlayerGameItem(itemId);
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

        public async Task UpdatePlayer()
        {
            //We may not have such kind of save
            //TODO: use throw to check where called this API

            //string playerString = JsonUtility.ToJson(GameManager.Instance.Players.Selected.PlayerGameItem);
            //string playerId = GameManager.Instance.UserId;
            //PlayerPrefs.SetString(DUMMY_PREFS_KEY + playerId, playerString);
        }


        /// <summary>
        /// Load the GameItems the player owns from the server
        /// </summary>
        /// <param name="handler"></param>
        public async Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId)
        {
            //yield return (HttpUtil.Get(GlobalConstants.SERVER_TISVC_PREFIX + "/{AppId}/GameItemMeta", webRequest =>
            //{
            //    GameItemMeta[] itemMetas = JsonUtility.FromJson<GameItemMeta[]>(webRequest.downloadHandler.text);
            //    //GameItemMeta[] itemMetas = JsonMapper.ToObject<GameItemMeta[]>(webRequest.downloadHandler.text);

            await (HttpUtil.GetDummyAsync(GlobalConstants.SERVER_TISVC_PREFIX + "/{AppId}/PlayerGameItem"));
            //TODO: Load form web then sync to local, or load from local if web is not available
            //List<PlayerGameItem> items = MockItems();
            //PopulatePlayerGameItems(playerId, items);
            return await RepoFactory.PlayerGameItemRepository.LoadPlayerGameItems(itemId);
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

            ////The HTY skin
            //List<GameItemEquipment> el1 = new List<GameItemEquipment>();

            //GameItemEquipment ce1 = new GameItemEquipment(MOCK_FISH2_ID, Slot.Root, MOCK_SKIN_ID2);
            //el1.Add(ce1);
            //GameItemEquipment ce2 = new GameItemEquipment(MOCK_FISH2_ID, Slot.RHand, MOCK_STICK_ID1);
            //el1.Add(ce2);

            //equipments.Add(MOCK_FISH2_ID, el1.ToArray());

            ////Fish1 skin
            //List<GameItemEquipment> el2 = new List<GameItemEquipment>();
            //GameItemEquipment ce22 = new GameItemEquipment(MOCK_FISH1_ID, Slot.Root, MOCK_SKIN_ID1);
            //el2.Add(ce22);

            //equipments.Add(MOCK_FISH1_ID, el2.ToArray());


            return equipments;
        }

        private List<PlayerGameItem> MockItems()
        {

            var items = new List<PlayerGameItem>();

            //PlayerGameItem pgi = new PlayerGameItem();
            //pgi.GiType = "Character";
            //pgi.GiId = "hty";
            //pgi.Id = MOCK_FISH2_ID;
            ////pgi.Position = new Vector3(0, 0 , 0);
            ////pgi.Rotation = new Vector3(0, 0, 0);// Quaternion.identity.eulerAngles;
            //pgi.PrefabType = GameItem.LocalisablePrefabType.Type1;
            //pgi.IsActive = true;
            //items.Add(pgi);

            ////The HTY skin
            //List<GameItemEquipment> el1 = new List<GameItemEquipment>();

            //GameItemEquipment ce1 = new GameItemEquipment(MOCK_FISH2_ID, Slot.Root, MOCK_SKIN_ID2);
            //el1.Add(ce1);
            //GameItemEquipment ce2 = new GameItemEquipment(MOCK_FISH2_ID, Slot.RHand, MOCK_STICK_ID1);
            //el1.Add(ce2);

            //pgi.CharacterEquipments = el1;

            //PlayerGameItem pgi2 = new PlayerGameItem();
            //pgi2.GiType = "Level";
            //pgi2.GiId = "forest";
            //pgi2.Id = "abcd";
            //items.Add(pgi2);

            //PlayerGameItem pgi3 = new PlayerGameItem();
            //pgi3.GiType = "AddressableGameItem";
            //pgi3.GiId = "Fish1Skin1";
            //pgi3.Id = MOCK_SKIN_ID1;
            //items.Add(pgi3);

            //PlayerGameItem pgi4 = new PlayerGameItem();
            //pgi4.GiType = "Character";
            //pgi4.GiId = "Fish1";
            //pgi4.Id = MOCK_FISH1_ID;
            ////pgi4.Position = new Vector3(0, 1, 0);
            ////pgi4.Rotation = new Vector3(0, 0, 0);// Quaternion.identity.eulerAngles;
            //pgi4.PrefabType = GameItem.LocalisablePrefabType.Type1;
            //pgi4.IsActive = true;
            //items.Add(pgi4);


            ////Fish1 skin
            //List<GameItemEquipment> el2 = new List<GameItemEquipment>();
            //GameItemEquipment ce22 = new GameItemEquipment(MOCK_FISH1_ID, Slot.Root, MOCK_SKIN_ID1);
            //el2.Add(ce22);
            //pgi4.CharacterEquipments = el2;

            //PlayerGameItem pgi5 = new PlayerGameItem();
            //pgi5.GiType = "AddressableGameItem";
            //pgi5.GiId = "AGI_Fish2Skin1";
            //pgi5.Id = MOCK_SKIN_ID2;
            //items.Add(pgi5);

            //PlayerGameItem pgi6 = new PlayerGameItem();
            //pgi6.GiType = "AddressableGameItem";
            //pgi6.GiId = "AGI_Stick1";
            //pgi6.Id = MOCK_STICK_ID1;
            //items.Add(pgi6);

            return items;
        }


        /// <summary>
        /// Set the GameItem status based on the PlayerGameItem
        /// </summary>
        /// <param name="playerGameItems"></param>
        /// <returns></returns>
        //private void PopulatePlayerGameItems(string playerId, List<PlayerGameItem> playerGameItems)
        //{
        //    foreach (PlayerGameItem playerGameItem in playerGameItems)
        //    {
        //        var gameItemManager = GameManager.Instance.GetIBaseGameItemManager(playerGameItem.GiType);
        //        GameItem addrItem = gameItemManager.BaseGetItem(playerGameItem.GiId);
        //        if (addrItem != null)
        //        {
        //            addrItem.IsUnlocked = true;
        //            //if (addrItem.GetType() == typeof(AddressableGameItem))
        //            //{
        //            //    //Addressable Game Items may have quantity
        //            //    addrItem.GetCounter(Constants.QUANTITY_COUNTER).Set(playerGameItem.Amount);
        //            //}
        //        }
        //        else
        //        {
        //            //Has GameItem not fetched, need to fetch again later
        //            Debug.LogWarning(string.Format("No GameItem available for {0}", playerGameItem.GameItem_name));
        //        }
        //    }
        //    if (GameManager.Instance.Players.Selected.GiId == playerId)
        //    {
        //        GameManager.Instance.Players.Selected.PlayerDto.OwnedItems = playerGameItems;
        //    }
        //    Debug.Log("PopulatePlayerGameItems Done");
        //}

        public async Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            return await RepoFactory.PlayerGameItemRepository.CreatePlayerGameItem(item);
        }

        public async Task AddChild(string parentId, PlayerGameItem child)
        {
            //TODO: call the service to persistent child and update the parent-child relationship
            child = await CreatePlayerGameItem(child);
            await RepoFactory.PlayerGameItemRepository.UpdateParentChildRelation(parentId, child.Id, true);
        }

        public async Task<PlayerGameItem> LoadCurrentScene()
        {
            return await RepoFactory.PlayerGameItemRepository.LoadCurrentScene();
        }
    }
}