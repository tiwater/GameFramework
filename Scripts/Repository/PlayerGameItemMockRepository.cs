using System;
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

public class PlayerGameItemMockRepository : BaseRepository, IPlayerGameItemRepository
{
    public Task<PlayerGameItem> CreatePlayerGameItem(PlayerGameItem item)
    {
        throw new NotImplementedException();
    }

    public async Task<PlayerGameItem> GetCurrentPlayerInstance()
    {
        PlayerGameItem player = new PlayerGameItem();
        player.Id = GameManager.Instance.UserId;
        return player;
    }

    public Task<PlayerGameItem> GetPlayerGameItem(string itemId)
    {
        throw new NotImplementedException();
    }

    public async Task<PlayerGameItem> LoadCurrentScene()
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
        character.IsActive = true;

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
        equipment.Props.Slotted = Slot.RHand;

        character.Equipments = new List<PlayerGameItem>();
        character.Equipments.Add(equipment);

        //Skin
        PlayerGameItem skin = new PlayerGameItem();
        skin.GiType = "AddressableGameItem";
        skin.GiId = "AGI_BallFishSkin1";
        skin.Id = "5";
        skin.PrefabType = LocalisablePrefabType.InGame;
        skin.IsActive = true;
        skin.Props.Slotted = Slot.Body;

        character.Equipments.Add(skin);

        //Debug.Log(JsonUtility.ToJson(scene));
        //Debug.Log(JsonConvert.SerializeObject(scene));
        return scene;
    }

    private List<PlayerGameItem> GenerateSeaweeds()
    {
        List<PlayerGameItem> seaweeds = new List<PlayerGameItem>();

        for(int i = 0; i < 150; i++)
        {
            PlayerGameItem seaweed = new PlayerGameItem();
            seaweed.GiType = "AddressableGameItem";
            seaweed.GiId = "AGI_Seaweeds";
            seaweed.Id = seaweed.GiId + i;
            seaweed.PrefabType = (i % (LocalisablePrefabType.Type7 - LocalisablePrefabType.Type1 + 1) + LocalisablePrefabType.Type1);
            seaweed.ExtraProps.SetVector3(Constants.PROP_KEY_POSITION, new Vector3(UnityEngine.Random.Range(-80, 80)/10.0f,
                0, UnityEngine.Random.Range(-70, 120)/10.0f));
            seaweed.ExtraProps.SetVector3(Constants.PROP_KEY_ROTATION, Quaternion.identity.eulerAngles);

            seaweeds.Add(seaweed);
            //seaweed.Props = Variables.FromJsonMapString(seaweed.Props.ToJsonMapString());
            //Debug.Log(seaweed.Props.ToJsonMapString());
        }

        return seaweeds;
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
}
