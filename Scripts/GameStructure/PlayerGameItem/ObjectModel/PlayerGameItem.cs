using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.Service;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

namespace GameFramework.GameStructure.PlayerGameItems.ObjectModel
{
    public class PlayerGameItem
    {
        public string Id;
        public string CustomName;
        public string PlayerId;
        public string GiId;
        public string GiType;
        public long Amount;
        public string HostDeviceId;
        public string CreatedDeviceId;
        public string ModelVersion;

        /// <summary>
        /// The equipment for each character
        /// </summary>
        //[SerializeField]
        //public List<GameItemEquipment> CharacterEquipments = new List<GameItemEquipment>();

        //[NonSerialized]
        public List<PlayerGameItem> Equipments { get; set; }

        /// <summary>
        /// The children PlayerGameItem
        /// </summary>
        //[SerializeField]
        //[NonSerialized]
        public List<PlayerGameItem> Children { get; set; }

        public LocalisablePrefabType PrefabType;
        public bool IsActive;

        public Props Props = new Props();

        /// <summary>
        /// A list of custom variables for this game item.
        /// </summary>
        public Variables.ObjectModel.Variables ExtraProps
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
        Variables.ObjectModel.Variables _variables = new Variables.ObjectModel.Variables();

        public string Category;

        public PlayerGameItem()
        {
            //Add default properties
            //if (Props.GetVector3(Constants.PROP_KEY_POSITION) == null)
            //{
            //    Props.SetVector3(Constants.PROP_KEY_POSITION, Vector3.zero);
            //}
            //if (Props.GetVector3(Constants.PROP_KEY_ROTATION) == null)
            //{
            //    Props.SetVector3(Constants.PROP_KEY_ROTATION, Quaternion.identity.eulerAngles);
            //}
        }

        public async Task AddChild(PlayerGameItem child)
        {
            if (child != this)
            {
                //Avoid loop
                await PlayerGameItemService.Instance.AddChild(Id, child);
            }
            else
            {
                Debug.LogError("Cannot add self as child");
            }
        }
    }

    //[Serializable]
    //public class EquipmentItem
    //{
    //    public string EquiptorId;
    //    public Slot EquipSlot;
    //    public PlayerGameItem Equipment;
    //}

    [Serializable]
    public class Props
    {
        public Slot Slotted;
        public float Health;
        public string Mood;
        public float Hungry;
        public long Age;
        public long Rank;
        public string Achievement;
    }
}