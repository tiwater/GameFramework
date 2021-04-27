using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace GameFramework.GameStructure.Util
{
    public class JsonUtil
    {
        public JsonUtil()
        {
        }

        public static string ExportGameItemMeta(GameItem gameItem)
        {
            return JsonConvert.SerializeObject(gameItem, Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = GameItemContractResolver.WhiteListInstance });
        }

        public static void PopulateGameItemMeta(string meta, GameItem gameItem)
        {
            //return JsonConvert.DeserializeObject<T>(meta, new GameItemConverter<T>());
            JsonConvert.PopulateObject(meta, gameItem);
        }

        public static string ListToJson<T>(List<T> l)
        {
            return JsonUtility.ToJson(new Serialization<T>(l));
        }

        public static List<T> ListFromJson<T>(string str)
        {
            return JsonUtility.FromJson<Serialization<T>>(str).ToList();
        }

        public static string DicToJson<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            return JsonUtility.ToJson(new Serialization<TKey, TValue>(dic));
        }

        public static Dictionary<TKey, TValue> DicFromJson<TKey, TValue>(string str)
        {
            return JsonUtility.FromJson<Serialization<TKey, TValue>>(str).ToDictionary();
        }
    }


    public class GameItemConverter<T> : CustomCreationConverter<T> where T : GameItem
    {
        public override T Create(Type objectType)
        {
            T gi = ScriptableObject.CreateInstance<T>();
            gi.InitialiseNonScriptableObjectValues(GameConfiguration.Instance, GameManager.Instance.Player, GameManager.Messenger);
            return gi;
        }
    }

    /// <summary>
    /// The ContractResolver allows to specify the properties to be serialized
    /// </summary>
    public class IgnorePropertiesContractResolver<T> : DefaultContractResolver
    {
        private List<string> propertiesList;
        private bool isBlackList;

        /// <summary>
        /// Ignore the proerties in blacklist, or only allow the properties in white list
        /// </summary>
        /// <param name="propertiesList"></param>
        /// <param name="isBlackList">True for blacklist and flase for whitelist</param>
        protected IgnorePropertiesContractResolver(List<string> propertiesList, bool isBlackList = true)
        {
            this.propertiesList = propertiesList;
            this.isBlackList = isBlackList;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            bool shouldSerialize = false;
            Type t = property.DeclaringType;

            if (t.IsSubclassOf(typeof(T)) || t.IsAssignableFrom(typeof(T)))
            {
                //Only handle the limitation on the root type
                if (isBlackList)
                {
                    //Blacklist
                    if (!propertiesList.Contains(property.PropertyName))
                    {
                        //Not in the list, so allow it
                        shouldSerialize = true;
                    }
                }
                else
                {
                    //Whitelist
                    if (propertiesList.Contains(property.PropertyName))
                    {
                        //In the list, so allow it
                        shouldSerialize = true;
                    }
                }
            }
            else
            {
                shouldSerialize = true;
            }

            property.ShouldSerialize =
                instance =>
                {
                    return shouldSerialize;
                };

            return property;
        }
    }

    /// <summary>
    /// The ContractResolver for GameItem
    /// </summary>
    public class GameItemContractResolver : IgnorePropertiesContractResolver<GameItem>
    {
        private static List<string> blackList = new List<string> {
        "Progress",
        "ProgressBest",
        "Sprite",
        "Score",
        "HighScore",
        "OldHighScore",
        "Coins",
        "Player"
    };

        private static List<string> whileList = new List<string>
    {
        "name",
        "LocalisableName",
        "LocalisableDescription",
        "ValueToUnlock",
        "Consumable",
        "DistinguishInstance"
    };

        public static readonly GameItemContractResolver BlackListInstance = new GameItemContractResolver(blackList, true);

        public static readonly GameItemContractResolver WhiteListInstance = new GameItemContractResolver(whileList, false);

        protected GameItemContractResolver(List<string> propertiesList, bool isBlackList = true) : base(propertiesList, isBlackList)
        {
        }
    }
}