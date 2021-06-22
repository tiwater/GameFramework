using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GameFramework.GameStructure.Util;
using GameFramework.Service;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

namespace GameFramework.GameStructure.PlayerGameItems.ObjectModel
{
    [Serializable]
    public class PlayerGameItem : BaseItem
    {
        public static readonly string ATTRS_SLOTTED= "Slotted";
        public static readonly string ATTRS_HEALTH = "Health";
        public static readonly string ATTRS_POSITION = "Position";
        public static readonly string ATTRS_ROTATION = "Rotation";
        public static readonly string ATTRS_IS_PLAYER = "IsPlayer";
        /// <summary>
        /// The user customized name for this item. The field Name is reserved for the localized name of the type
        /// </summary>
        public string CustomName;
        public string PlayerId;
        public long Amount;
        public string HostDeviceId;
        public string CreatedDeviceId;
        public string ModelVersion;

        /// <summary>
        /// The equipment for each character
        /// </summary>
        [NonSerialized]
        public List<PlayerGameItem> Equipments;

        /// <summary>
        /// The children PlayerGameItem
        /// </summary>
        [NonSerialized]
        public List<PlayerGameItem> Children;

        public LocalisablePrefabType PrefabType;
        public bool IsActive;

        public Dictionary<string, string> Attrs = new Dictionary<string, string>();

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

        public List<string> CompareWith(PlayerGameItem that)
        {
            List<string> differents = ObjectUtil.CompareObjects(this, that);
            differents.AddRange(ObjectUtil.CompareObjects(this.Attrs, that.Attrs));
            if(CompareLists(this.Children, that.Children) != 0)
            {
                //Children are different
                differents.Add("Children");
            }
            if (CompareLists(this.Equipments, that.Equipments) != 0)
            {
                //Equipments are different
                differents.Add("Equipments");
            }
            List<string> differentsProps = this.ExtraProps.CompareWith(that.ExtraProps);
            if (differentsProps.Count > 0)
            {
                //Add the ExtraProps property name
                differents.Add("ExtraProps");
                //And add the difference inside ExtraProps
                differents.AddRange(differentsProps);
            }

            return differents;
        }

        private int CompareLists(List<PlayerGameItem> thisChildren, List<PlayerGameItem> thatChildren)
        {
            //Check whether the List is empty
            if(thisChildren==null || thisChildren.Count==0 )
            {
                if (thatChildren == null || thatChildren.Count == 0)
                {
                    return 0;
                } else
                {
                    return -1;
                }
            }
            if(thatChildren == null || thatChildren.Count == 0)
            {
                return 1;
            }
            //Check the list size
            if (thisChildren.Count > thatChildren.Count)
            {
                return 1;
            } else if (thisChildren.Count < thatChildren.Count)
            {
                return -1;
            }
            //Sort the list to ease the compare
            thisChildren.Sort((x, y) => x.Id.CompareTo(y.Id));
            thatChildren.Sort((x, y) => x.Id.CompareTo(y.Id));
            for (int i = 0; i < thisChildren.Count; i++)
            {
                //Compare the id on each side
                if(thisChildren[i].Id == thatChildren[i].Id)
                {
                    List<string> differents = thisChildren[i].CompareWith(thisChildren[i]);
                    if(differents!=null && differents.Count > 0)
                    {
                        //The two children are different
                        //TODO: find a method to mark the comparison of the two objects, e.g. add a CompareTo method
                        //Now we just return 1 to mark the list are different
                        return 1;
                    }
                } else
                {
                    //Different Id
                    if (thisChildren[i].Id == null)
                    {
                        //This id is null, consider it is less
                        return -1;
                    }
                    else
                    {
                        //Compare the Ids
                        return thisChildren[i].Id.CompareTo(thatChildren[i].Id);
                    }
                }
            }
            //No difference
            return 0;

        }

        public string GetAttr(string key)
        {
            string value;
            Attrs.TryGetValue(key, out value);
            return value;
        }

        public void SetAttr(string key, string value)
        {
            Attrs[key] = value;
        }

        private T GetTypedAttr<T>(string key, T defaultValue)
        {
            string value = null;
            Attrs.TryGetValue(key, out value);
            return ObjectUtil.TryParse<T>(value, defaultValue);
        }

        public float GetFloatAttr(string key, float defaultValue = 0)
        {
            return GetTypedAttr<float>(key, defaultValue);
        }

        public void SetFloatAttr(string key, float value)
        {
            Attrs[key] = value.ToString();
        }

        public bool GetBoolAttr(string key, bool defaultValue = false)
        {
            return GetTypedAttr<bool>(key, defaultValue);
        }

        public void SetBoolAttr(string key, bool value)
        {
            Attrs[key] = value.ToString();
        }

        public Vector3 GetVector3Attr(string key, Vector3 defaultValue)
        {
            string value = null;
            if(Attrs.TryGetValue(key, out value))
            {
                Vector3 vector3 = JsonUtility.FromJson<Vector3>(value);
                return vector3;
            } else 
            {
                //Doesn't have the value, return the default
                return defaultValue;
            }
        }

        public void SetVector3Attr(string key, Vector3 value)
        {
            Attrs[key] = JsonUtility.ToJson(value);
        }

        public Quaternion GetQuaternionAttr(string key, Quaternion defaultValue)
        {
            string value = null;
            if (Attrs.TryGetValue(key, out value))
            {
                Quaternion vector3 = JsonUtility.FromJson<Quaternion>(value);
                return vector3;
            }
            else
            {
                //Doesn't have the value, return the default
                return defaultValue;
            }
        }

        public void SetQuaternionAttr(string key, Quaternion value)
        {
            Attrs[key] = JsonUtility.ToJson(value);
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