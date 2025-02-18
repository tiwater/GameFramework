﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Util;
using GameFramework.GameStructure.Variables.ObjectModel;
using GameFramework.Repository;
using Newtonsoft.Json;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

namespace GameFramework.Repository
{
    public class PlayerGameItemMockRepository : BaseRepository, IPlayerGameItemRepository
    {
        public virtual Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<PlayerGameItem> GetCurrentPlayerInstance()
        {
            PlayerGameItem player = new PlayerGameItem();
            player.Id = GameManager.Instance.UserId;
            return player;
        }

        public async virtual Task<PlayerGameItem> GetPlayerGameItem(string itemId)
        {

            PlayerGameItem character = new PlayerGameItem();
            character.GiType = "Character";
            character.GiId = "Character_BallFish";
            character.Id = itemId;
            character.PrefabType = LocalisablePrefabType.Type1;
            character.IsActive = true;
            character.SetFloatAttr(PlayerGameItem.ATTRS_HEALTH, UnityEngine.Random.Range(0, 100));

            return character;
        }

        public async virtual Task<PlayerGameItem> LoadCurrentScene(string theme)
        {
            PlayerGameItem scene = new PlayerGameItem();
            scene.GiType = "Level";
            scene.GiId = "Level_sea1";
            scene.Id = "1";
            scene.PrefabType = LocalisablePrefabType.InGame;

            //Generate the seaweeds
            scene.Children = GenerateSeaweeds();

            //Generate character
            PlayerGameItem character = new PlayerGameItem();
            character.GiType = "Character";
            character.GiId = "Character_Turtle";
            character.Id = "2";
            character.PrefabType = LocalisablePrefabType.Type1;
            character.SetBoolAttr(PlayerGameItem.ATTRS_IS_PLAYER, true);

            scene.Children.Add(character);

            //Generate character
            character = new PlayerGameItem();
            character.GiType = "Character";
            character.GiId = "Character_BallFish";
            character.Id = "3";
            character.PrefabType = LocalisablePrefabType.Type1;
            character.IsActive = false;

            scene.Children.Add(character);

            //Equipment: stick
            PlayerGameItem equipment = new PlayerGameItem();
            equipment.GiType = "AddressableGameItem";
            equipment.GiId = "AGI_Stick1";
            equipment.Id = "4";
            equipment.PrefabType = LocalisablePrefabType.InGame;
            equipment.IsActive = true;
            equipment.SetAttr(PlayerGameItem.ATTRS_SLOTTED, Enum.GetName(typeof(Slot), Slot.RHand));

            character.Equipments = new List<PlayerGameItem>();
            character.Equipments.Add(equipment);

            //Skin
            PlayerGameItem skin = new PlayerGameItem();
            skin.GiType = "AddressableGameItem";
            skin.GiId = "AGI_BallFishSkin1";
            skin.Id = "5";
            skin.PrefabType = LocalisablePrefabType.InGame;
            skin.IsActive = true;
            skin.SetAttr(PlayerGameItem.ATTRS_SLOTTED, Enum.GetName(typeof(Slot), Slot.Body));

            character.Equipments.Add(skin);

            //Debug.Log(JsonUtility.ToJson(scene));
            //Debug.Log(JsonConvert.SerializeObject(scene));
            return scene;
        }

        protected List<PlayerGameItem> GenerateSeaweeds()
        {
            List<PlayerGameItem> seaweeds = new List<PlayerGameItem>();

            for (int i = 0; i < 20; i++)
            {
                PlayerGameItem seaweed = new PlayerGameItem();
                seaweed.GiType = "AddressableGameItem";
                seaweed.GiId = "AGI_Seaweeds";
                seaweed.Id = seaweed.GiId + i;
                seaweed.PrefabType = (i % (LocalisablePrefabType.Type7 - LocalisablePrefabType.Type1 + 1) + LocalisablePrefabType.Type1);
                seaweed.SetVector3Attr(PlayerGameItem.ATTRS_POSITION, new Vector3(UnityEngine.Random.Range(-80, 80) / 10.0f,
                    0, UnityEngine.Random.Range(-70, 120) / 10.0f));
                seaweed.SetVector3Attr(PlayerGameItem.ATTRS_ROTATION, Quaternion.identity.eulerAngles);

                seaweeds.Add(seaweed);
                //seaweed.Props = Variables.FromJsonMapString(seaweed.Props.ToJsonMapString());
                //Debug.Log(seaweed.Props.ToJsonMapString());
            }

            return seaweeds;
        }

        public async virtual Task<List<PlayerGameItem>> LoadPlayerGameItems(string itemId)
        {
            var items = new List<PlayerGameItem>();
            return items;
        }

        public virtual Task<string> LoadToken()
        {
            throw new NotImplementedException();
        }

        public virtual Task StoreToken(string token)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateParentChildRelation(string parentId, string childId, bool add)
        {
            throw new NotImplementedException();
        }

        static bool hasEquipment = false;
        public async Task<List<PlayerGameItem>> GetEquipments(string itemId)
        {

            var Equipments = new List<PlayerGameItem>();

            if (hasEquipment)
            {
                PlayerGameItem equipment = new PlayerGameItem();
                equipment.GiType = "AddressableGameItem";
                equipment.GiId = "AGI_Stick1";
                equipment.Id = "4";
                equipment.PrefabType = LocalisablePrefabType.InGame;
                equipment.IsActive = true;
                equipment.SetAttr(PlayerGameItem.ATTRS_SLOTTED, Enum.GetName(typeof(Slot), Slot.RHand));
                Equipments.Add(equipment);

                //Skin
                PlayerGameItem skin = new PlayerGameItem();
                skin.GiType = "AddressableGameItem";
                skin.GiId = "AGI_BallFishSkin1";
                skin.Id = "5";
                skin.PrefabType = LocalisablePrefabType.InGame;
                skin.IsActive = true;
                skin.SetAttr(PlayerGameItem.ATTRS_SLOTTED, Enum.GetName(typeof(Slot), Slot.Body));

                Equipments.Add(skin);
            }
            hasEquipment = !hasEquipment;
            return Equipments;
        }
    }
}