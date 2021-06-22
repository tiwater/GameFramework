using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Creation;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Variables.ObjectModel;
using Grpc.Core;
using Userinfo;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

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
            Channel channel = ChannelFactory.GetChannel();

            var client = new UserInfo.UserInfoClient(channel);

            var reply = await client.GetUserInfoAsync(new Google.Protobuf.WellKnownTypes.Empty());

            channel.ShutdownAsync().Wait();

            PlayerGameItem player = new PlayerGameItem();
            player.Id = reply.Uid;
            player.CustomName = reply.Nickname;

            return player;
        }

        public Task<PlayerGameItem> GetPlayerGameItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerGameItem> LoadCurrentScene(string theme)
        {

            Channel channel = ChannelFactory.GetChannel();

            var client = new CreationProvider.CreationProviderClient(channel);

            var request = new GetCreationRequest();

            request.World = theme.ToLower();

            var reply = await client.GetCreationAsync(request);

            channel.ShutdownAsync().Wait();

            return PopulatePlayerGameItem(reply);
        }

        private PlayerGameItem PopulatePlayerGameItem(Creation.Creation creation)
        {
            PlayerGameItem item = null;
            if (creation != null)
            {

                //Copy the fields
                item = new PlayerGameItem();
                item.GiType = creation.CreationType;
                item.GiId = creation.Template;
                item.Id = creation.Id;
                Enum.TryParse(creation.PrefabType, out item.PrefabType);
                item.CustomName = creation.Name;
                item.ModelVersion = creation.SchemaVersion;
                item.IsActive = "active" == creation.Status;
                item.Category = creation.Category;

                //The props the logic cares
                if (creation.Attrs != null)
                {
                    foreach(var pair in creation.Attrs)
                    {
                        item.Attrs.Add(pair.Key, pair.Value);
                    }
                }
                //The extra props
                if (creation.ExtraProps != null)
                {
                    item.ExtraProps = Variables.FromDict(creation.ExtraProps);
                }
                //The children
                if (creation.Children != null && creation.Children.Count > 0)
                {

                    var children = new List<PlayerGameItem>();
                    var equipments = new List<PlayerGameItem>();
                    foreach (var child in creation.Children)
                    {
                        var subItem = PopulatePlayerGameItem(child);
                        if (subItem.Category == "uw:pet:equipment")
                        {
                            //Equipment
                            if (subItem.IsActive)
                            {
                                //Only equip the actived item
                                equipments.Add(subItem);
                            }
                        }
                        else
                        {
                            //Normal item
                            children.Add(PopulatePlayerGameItem(child));
                        }
                    }
                    if (children.Count > 0)
                    {
                        item.Children = children;
                    }
                    if (equipments.Count > 0)
                    {
                        item.Equipments = equipments;
                    }
                }
            }
            return item;
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

        public Task<List<PlayerGameItem>> GetEquipments(string itemId)
        {
            throw new NotImplementedException();
        }
    }
}