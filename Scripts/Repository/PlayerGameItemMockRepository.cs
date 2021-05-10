using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Util;
using GameFramework.Repository;
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
        scene.Children = await GenerateSeaweeds();

        //Generate character
        PlayerGameItem character = new PlayerGameItem();
        character.GiType = "Character";
        character.GiId = "Character_BallFish";
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
        return scene;
    }

    private async Task<List<PlayerGameItem>> GenerateSeaweeds()
    {
        List<PlayerGameItem> seaweeds = new List<PlayerGameItem>();

        for(int i = 0; i < 80; i++)
        {
            PlayerGameItem seaweed = new PlayerGameItem();
            seaweed.GiType = "AddressableGameItem";
            seaweed.GiId = "AGI_Seaweeds";
            seaweed.Id = seaweed.GiId + i;
            seaweed.PrefabType = (i % (LocalisablePrefabType.Type7 - LocalisablePrefabType.Type1 + 1) + LocalisablePrefabType.Type1);
            seaweed.Props.SetVector3(Constants.PROP_KEY_POSITION, new Vector3(UnityEngine.Random.Range(-60, 60)/10.0f,
                0, UnityEngine.Random.Range(-50, 100)/10.0f));

            seaweeds.Add(seaweed);
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
