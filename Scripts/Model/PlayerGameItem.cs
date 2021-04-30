using System;
using System.Collections.Generic;
using GameFramework.GameStructure.Model;
using GameFramework.GameStructure.Util;
using GameFramework.GameStructure.Variables.ObjectModel;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;


[Serializable]
public class PlayerGameItem
{
    public string Id;
    public string PlayerId;
    public string GiId { get {
            return GameItem_name.Substring(GameItem_name.IndexOf('_') + 1);
        }
    }
    public string GameItem_name;
    public string GiType;
    public long Amount;
    public string HostDeviceId;
    public string CreatedDeviceId;

    /// <summary>
    /// The equipment for each character
    /// </summary>
    [SerializeField]
    public List<GameItemEquipment> CharacterEquipments = new List<GameItemEquipment>();

    /// <summary>
    /// The children PlayerGameItem
    /// </summary>
    [SerializeField]
    public List<PlayerGameItem> Children = new List<PlayerGameItem>();

    public LocalisablePrefabType PrefabType;
    public bool IsActive;

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

    public PlayerGameItem()
    {
        //Add default properties
        if (Props.GetVector3(Constants.PROP_KEY_POSITION) == null)
        {
            Props.SetVector3(Constants.PROP_KEY_POSITION, Vector3.zero);
        }
        if (Props.GetVector3(Constants.PROP_KEY_ROTATION) == null)
        {
            Props.SetVector3(Constants.PROP_KEY_ROTATION, Quaternion.identity.eulerAngles);
        }
    }
}
