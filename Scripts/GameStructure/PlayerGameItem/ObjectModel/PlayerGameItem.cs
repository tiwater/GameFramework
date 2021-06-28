using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameFramework.GameStructure.Util;
using GameFramework.Service;
using Newtonsoft.Json;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

namespace GameFramework.GameStructure.PlayerGameItems.ObjectModel
{
    [Serializable]
    public class PlayerGameItem : BaseItem
    {
        public const string ATTRS_SLOTTED= "Slotted";
        public const string ATTRS_HEALTH = "Health";
        public const string ATTRS_MOOD = "Mood";
        public const string ATTRS_HUNGRY = "Hungry";
        public const string ATTRS_POSITION = "Position";
        public const string ATTRS_ROTATION = "Rotation";
        public const string ATTRS_IS_PLAYER = "IsPlayer";
        public const string ATTRS_INDEX = "Index";
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

        /// <summary>
        /// Store customized attributes, all in string format
        /// </summary>
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

        public bool IsPlayerCharacter()
        {
            return GetBoolAttr(ATTRS_IS_PLAYER);
        }

        /// <summary>
        /// Get the attributes in Attrs
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAttr(string key)
        {
            string value;
            Attrs.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Store attribute value into Attrs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAttr(string key, string value)
        {
            Attrs[key] = value;
        }

        /// <summary>
        /// The generic method to get attribute value from Attrs other than string type,
        /// the target type must support parse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private T GetParsableTypedAttr<T>(string key, T defaultValue)
        {
            string value = null;
            Attrs.TryGetValue(key, out value);
            return ObjectUtil.TryParse<T>(value, defaultValue);
        }

        public float GetFloatAttr(string key, float defaultValue = 0)
        {
            return GetParsableTypedAttr<float>(key, defaultValue);
        }

        public void SetFloatAttr(string key, float value)
        {
            Attrs[key] = value.ToString();
        }

        public bool GetBoolAttr(string key, bool defaultValue = false)
        {
            return GetParsableTypedAttr<bool>(key, defaultValue);
        }

        public void SetBoolAttr(string key, bool value)
        {
            Attrs[key] = value.ToString();
        }

        public int GetIntAttr(string key, int defaultValue = 0)
        {
            return GetParsableTypedAttr<int>(key, defaultValue);
        }

        public void SetIntAttr(string key, int value)
        {
            Attrs[key] = value.ToString();
        }

        /// <summary>
        /// The generic method to get attribute value from Attrs other than string type,
        /// the target type doesn't support parse, but must support JsonUtility
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetTypedAttr<T>(string key, T defaultValue)
        {
            string value = null;
            if (Attrs.TryGetValue(key, out value))
            {
                T vector3 = JsonUtility.FromJson<T>(value);
                return vector3;
            }
            else
            {
                //Doesn't have the value, return the default
                return defaultValue;
            }
        }

        public Vector3 GetVector3Attr(string key, Vector3 defaultValue = default(Vector3))
        {
            return GetTypedAttr<Vector3>(key, defaultValue);
        }

        public void SetVector3Attr(string key, Vector3 value)
        {
            Attrs[key] = JsonUtility.ToJson(value);
        }

        public Quaternion GetQuaternionAttr(string key, Quaternion defaultValue = default(Quaternion))
        {
            return GetTypedAttr<Quaternion>(key, defaultValue);
        }

        public void SetQuaternionAttr(string key, Quaternion value)
        {
            Attrs[key] = JsonUtility.ToJson(value);
        }

        /// <summary>
        /// Due to limitation of the JsonUtility and JsonConvert, deserialize the entity
        /// by customized method
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            string json = JsonUtility.ToJson(this);
            StringBuilder sb = new StringBuilder();
            sb.Append(json);
            //Remove the last "}"
            sb.Length --;

            //Attrs
            sb.Append(", \"Attrs\":");
            sb.Append(JsonConvert.SerializeObject(Attrs));

            //Append the generated string for ExtraProps in Dictionary format
            sb.Append(", \"ExtraProps\":");
            sb.Append(ExtraProps.ToJsonMapString());

            if (Children != null)
            {
                //The children nodes
                sb.Append(", \"Children\":[");

                foreach (var child in Children)
                {
                    sb.Append(child.ToJson());
                    sb.Append(",");
                }
                if (sb[sb.Length - 1] == ',')
                {
                    sb.Length--;
                }
                sb.Append("]");
            }

            if (Equipments != null)
            {
                //The equipments node
                sb.Append(", \"Equipments\":[");

                foreach (var child in Children)
                {
                    sb.Append(child.ToJson());
                    sb.Append(",");
                }
                if (sb[sb.Length - 1] == ',')
                {
                    sb.Length--;
                }
                sb.Append("]");
            }
            sb.Append("}");


            return sb.ToString();
        }
    }
}