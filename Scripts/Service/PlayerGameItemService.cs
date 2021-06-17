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

        private IPlayerGameItemRepository pgiRepository;

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

        private PlayerGameItemService()
        {
            pgiRepository = RepoFactory.GetRepository<IPlayerGameItemRepository>();
        }

        public async Task<string> LoadToken()
        {
            return await pgiRepository.LoadToken();
        }

        public async Task StoreToken(string token)
        {
            await pgiRepository.StoreToken(token);
        }


        public async Task<PlayerGameItem> GetCurrentPlayerInstance()
        {

            return await pgiRepository.GetCurrentPlayerInstance();
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
            return await pgiRepository.GetPlayerGameItem(itemId);
        }

        public async Task<List<PlayerGameItem>> GetEquipments(string itemId)
        {
            return await pgiRepository.GetEquipments(itemId);
        }

        public async Task UpdatePlayer()
        {
        }


        /// <summary>
        /// Load the GameItems the player owns from the server
        /// </summary>
        /// <param name="handler"></param>
        public async Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId)
        {

            return await pgiRepository.LoadPlayerGameItems(itemId);
        }

        public async Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            return await pgiRepository.CreatePlayerGameItem(item);
        }

        public async Task AddChild(string parentId, PlayerGameItem child)
        {
            //TODO: call the service to persistent child and update the parent-child relationship
            child = await CreatePlayerGameItem(child);
            await pgiRepository.UpdateParentChildRelation(parentId, child.Id, true);
        }

        public async Task<PlayerGameItem> LoadCurrentScene(string theme)
        {
            return await pgiRepository.LoadCurrentScene(theme.ToLower());
        }
    }
}