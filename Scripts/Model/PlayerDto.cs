using System;
using System.Collections.Generic;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Variables.ObjectModel;
using UnityEngine;

[Serializable]
public class PlayerDto
{
    public string Id;
    public string AppId;

    [SerializeField]
    //public List<PlayerGameItem> OwnedItems;
    public List<PlayerGameItem> OwnedItems;


    /// <summary>
    /// The equipment for each character
    /// </summary>
    //[SerializeField]
    //public Dictionary<string, CharacterEquipment[]> CharacterEquipments;

    /// <summary>
    /// A list of custom variables for this game item.
    /// </summary>
    public Variables Props
    {
        get
        {
            return _variables;
        }
        set
        {
            _variables = value;
        }
    }
    [Tooltip("A list of custom variables for this game item.")]
    [SerializeField]
    Variables _variables = new Variables();

    public PlayerGameItem GetPlayerGameItemById(string id)
    {
        if (OwnedItems != null)
        {
            foreach (var item in OwnedItems)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
        }
        return null;
    }
}
