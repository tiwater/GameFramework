using UnityEngine;
using GameFramework.GameStructure.GameItems.ObjectModel;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using GameFramework.GameStructure.Model;
using GameFramework.GameStructure.Characters.ObjectModel;
using static GameFramework.Service.AddressableResService;
using GameFramework.Service;
using static GameFramework.GameStructure.PlayerGameItems.ObjectModel.GameItemEquipment;

namespace GameFramework.GameStructure.AddressableGameItems.ObjectModel
{
    /// <summary>
    /// Extend Game Item
    /// </summary>
    [CreateAssetMenu(fileName = "AddressableGameItem_x", menuName = "Game Framework/Addressable Game Item")]
    public class AddressableGameItem : GameItem
    {
        /// <summary>
        /// Override for this GameItem type
        /// </summary>
        public override string IdentifierBase { get { return "AGI"; } }

        /// <summary>
        /// Override for this GameItem type
        /// </summary>
        public override string IdentifierBasePrefs { get { return "AGI"; } }

        #region Editor Parameters

        /// <summary>
        /// The Addressable Name for the contained resource
        /// </summary>
        public string AddressableName;

        /// <summary>
        /// The Addressable Label for the contained resource
        /// </summary>
        public string AddressableLabel;

        /// <summary>
        /// The thumbnail URL of the GameItem to display in eshop
        /// </summary>
        public string ThumbnailUrl;

        /// <summary>
        /// For the wearable items, the equipment slot which support
        /// </summary>
        [Tooltip("The Slot this AddressGameItem supports.")]
        [SerializeField]
        public List<Slot> SupportedSlots = new List<Slot>();

        /// <summary>
        /// The type of the GameItem content
        /// </summary>
        public AddressableGameItemMeta.ContentType ContentType
        {
            get
            {
                return _contentType;
            }
            set
            {
                _contentType = value;
            }
        }
        [Tooltip("The type of the GameItem content.")]
        [SerializeField]
        AddressableGameItemMeta.ContentType _contentType;

        /// <summary>
        /// The GameItem this asset supports.
        /// </summary>
        public List<SupportItemInfo> SupportItems
        {
            get
            {
                return _supportItems;
            }
        }
        [Tooltip("The GameItems that this AddressGameItem supports.")]
        [SerializeField]
        List<SupportItemInfo> _supportItems = new List<SupportItemInfo>();


        #endregion Editor Parameters


        /// <summary>
        /// For Texture, the default behavior is trying to replace the material has the same texture name on the target object or children
        /// </summary>
        /// <param name="target"></param>
        /// <param name="resourceName"></param>
        /// <param name="resource"></param>
        private void TryToApplyTexture2DOnChildren(GameObject target, string resourceName, Texture2D resource)
        {
            if (target != null)
            {
                SkinnedMeshRenderer smr = target.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    foreach (Material material in smr.materials)
                    {
                        if (material.mainTexture != null)
                        {
                            //Debug.Log(string.Format("Model texture name:{0} : {1}", material.mainTexture.name, resourceName));
                            if (material.mainTexture.name.Equals(resourceName))
                            {
                                material.mainTexture = resource;
                            }
                        }
                    }
                }
                //Check Children
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    TryToApplyTexture2DOnChildren(target.transform.GetChild(i).gameObject, resourceName, resource);
                }
            }
        }

        /// <summary>
        /// Apply the pass in resource to the target gameobject
        /// </summary>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        /// <param name="type"></param>
        /// <param name="instantiateInWorldSpace"></param>
        private void ApplyResource(GameObject target, string name, object resource, Type type, bool instantiateInWorldSpace, ResourceLoadedCallback callback)
        {
            if (type == typeof(Texture2D))
            {
                //Texture
                TryToApplyTexture2DOnChildren(target, name, (Texture2D)resource);
            }
            else if (type == typeof(GameObject))
            {
                //Prefab
                if (target == null)
                {
                    resource = Instantiate((GameObject)resource);
                }
                else
                {
                    resource = Instantiate((GameObject)resource, target.transform, instantiateInWorldSpace);
                }
            }
            else
            {
                Debug.LogWarning(string.Format("Unsupported type {0} of resource {1}, please add the code to handle it!!!",
                    type, name));
            }
            if (callback != null)
            {
                callback(name, resource, type);
            }
        }

        /// <summary>
        /// Apply the pass in resource to the target gameobject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        /// <param name="instantiateInWorldSpace"></param>
        /// <param name="callback"></param>
        private void ApplyResource<T>(GameObject target, string name, object resource, bool instantiateInWorldSpace, ResourceLoadedCallback<T> callback)
        {
            ApplyResource(target, name, resource, typeof(T), instantiateInWorldSpace, (name, resource, type) =>
            {
                if (callback != null)
                {
                    callback(name, (T)resource);
                }
            });
        }

        /// <summary>
        /// Apply the pass in resource to the target gameobject
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void ApplyResource(GameObject target, Vector3 position, Quaternion rotation, string name, object resource, Type type, ResourceLoadedCallback callback)
        {
            if (type == typeof(Texture2D))
            {
                //Texture
                TryToApplyTexture2DOnChildren(target, name, (Texture2D)resource);
            }
            else if (type == typeof(GameObject))
            {
                //Prefab
                if (target == null)
                {
                    resource = Instantiate((GameObject)resource, position, rotation);
                }
                else
                {
                    resource = Instantiate((GameObject)resource, position, rotation, target.transform);
                }
            }
            else
            {
                Debug.LogWarning(string.Format("Unsupported type {0} of resource {1}, please add the code to handle it!!!",
                    type, name));
            }
            if (callback != null)
            {
                callback(name, resource, type);
            }
        }

        /// <summary>
        /// Apply the pass in resource to the target gameobject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        /// <param name="callback"></param>
        public void ApplyResource<T>(GameObject target, Vector3 position, Quaternion rotation, string name, object resource, ResourceLoadedCallback<T> callback)
        {
            ApplyResource(target, position, rotation, name, resource, typeof(T), (name, resource, type) =>
            {
                if (callback != null)
                {
                    callback(name, (T)resource);
                }
            });
        }

        public void Apply(GameObject target, Slot slot, LocalisablePrefabType localisablePrefabType, bool instantiateInWorldSpace = false)
        {
            target = FindSlotTarget(target, slot) ?? target;
            if (ContentType == AddressableGameItemMeta.ContentType.Skin)
            {
                //Skin is texture
                foreach (var texture in _localisableTextures)
                {
                    //TODO: Now compare the enum string, should find a better way
                    if (Enum.GetName(typeof(LocalisableTextureType), texture.LocalisableTextureType)==
                        Enum.GetName(typeof(LocalisablePrefabType), localisablePrefabType))
                    {
                        TryToApplyTexture2DOnChildren(target, texture.LocalisableTexture.GetTexture().name,
                            (Texture2D)texture.LocalisableTexture.GetTexture());
                    }
                }
            }
            else
            {
                //Others are entity
                foreach (var prefab in _localisablePrefabs)
                {
                    //If in supported slot
                    if(SupportedSlots.Contains(Slot.All) || SupportedSlots.Contains(slot))
                    {
                        //Matched prefab type
                        if(localisablePrefabType == prefab.LocalisablePrefabType)
                        {
                            GameObject resource;
                            //Prefab
                            if (target == null)
                            {
                                resource = Instantiate((GameObject)prefab.LocalisablePrefab.GetPrefab());
                            }
                            else
                            {
                                resource = Instantiate((GameObject)prefab.LocalisablePrefab.GetPrefab(), target.transform, instantiateInWorldSpace);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        /// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        /// object will be a stand alone object
        /// </summary>
        /// <param name="target">The parent of the resources</param>
        /// <param name="names">The name list of the resources to be applied</param>
        /// <param name="label">The label of the resources to be applied. If either names and label are not specified, then the Package of the AddressableGameItem will be used as the label to filter the resources</param>
        /// <param name="instantiateInWorldSpace"></param>
        /// <param name="callback"></param>
        public async Task Apply(GameObject target, Slot slot, List<string> names = null, string label = null, bool instantiateInWorldSpace = false, ResourceLoadedCallback callback = null)
        {
            bool isEmptyName = StringUtil.IsListEmpty(names);
            target = FindSlotTarget(target, slot) ?? target;
            if (isEmptyName && string.IsNullOrEmpty(label))
            {
                Debug.LogError("Names and Label must have at least one piece of valid data");
            }
            else
            {

                if (string.IsNullOrEmpty(label))
                {
                    //Label is null
                    foreach (string key in names)
                    {

                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsAsync(key);
                        await Task.WhenAll(rls.Select(rl => AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
                        {
                            ApplyResource(target, name, resource, type, instantiateInWorldSpace, callback);
                        })));
                    }
                }
                else
                {

                    //Apply resources under same label as a batch
                    var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(label);
                    await Task.WhenAll(rls.Where(rl => (isEmptyName || names.Contains(rl.PrimaryKey)))
                        .Select(rl => AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
                        {
                            ApplyResource(target, name, resource, type, instantiateInWorldSpace, callback);
                        })));
                }
            }
        }

        /// <summary>
        /// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        /// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        /// object will be a stand alone object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="names"></param>
        /// <param name="label"></param>
        /// <param name="instantiateInWorldSpace"></param>
        /// <param name="callback"></param>
        public async Task Apply<T>(GameObject target, Slot slot, List<string> names = null, string label = null, bool instantiateInWorldSpace = false, ResourceLoadedCallback<T> callback = null)
        {
            target = FindSlotTarget(target, slot) ?? target;
            bool isEmptyName = StringUtil.IsListEmpty(names);
            if (isEmptyName && string.IsNullOrEmpty(label))
            {
                Debug.LogError("Names and Label must have at least one piece of valid data");
            }
            else
            {

                if (string.IsNullOrEmpty(label))
                {
                    //Label is null
                    foreach (string key in names)
                    {
                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsAsync(key);
                        Dictionary<string, Task<T>> tasks = new Dictionary<string, Task<T>>();
                        foreach (var rl in rls)
                        {
                            if (rl.ResourceType == typeof(T))
                            {
                                //Load resources
                                tasks.Add(rl.PrimaryKey, AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl));
                            }
                        }
                        //Wait for completion
                        await Task.WhenAll(tasks.Values);
                        foreach (var pair in tasks)
                        {
                            //Apply
                            ApplyResource<T>(target, pair.Key, await pair.Value, instantiateInWorldSpace, callback);
                        }
                    }
                }
                else
                {


                    //Apply resources under same label as a batch
                    var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(label);
                    Dictionary<string, Task<T>> tasks = new Dictionary<string, Task<T>>();
                    foreach (var rl in rls)
                    {
                        if (rl.ResourceType == typeof(T) && (isEmptyName || names.Contains(rl.PrimaryKey)))
                        {
                            //Load resources
                            tasks.Add(rl.PrimaryKey, AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl));
                        }
                    }
                    //Wait for completion
                    await Task.WhenAll(tasks.Values);
                    foreach (var pair in tasks)
                    {
                        //Apply
                        ApplyResource<T>(target, pair.Key, await pair.Value, instantiateInWorldSpace, callback);
                    }
                }
            }
        }

        /// <summary>
        /// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        /// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        /// object will be a stand alone object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="names"></param>
        /// <param name="label"></param>
        /// <param name="callback"></param>
        public async Task Apply(GameObject target, Slot slot, Vector3 position, Quaternion rotation, List<string> names = null, string label = null, ResourceLoadedCallback callback = null)
        {
            target = FindSlotTarget(target, slot) ?? target;
            bool isEmptyName = StringUtil.IsListEmpty(names);
            if (isEmptyName && string.IsNullOrEmpty(label))
            {
                Debug.LogError("Names and Label must have at least one piece of valid data");
            }
            else
            {
                if (string.IsNullOrEmpty(label))
                {
                    //Label is null
                    foreach (string key in names)
                    {

                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsAsync(key);
                        await Task.WhenAll(rls.Select(rl => AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
                        {
                            ApplyResource(target, position, rotation, name, resource, type, callback);
                        })));
                    }
                }
                else
                {

                    //Apply resources under same label as a batch
                    var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(label);
                    await Task.WhenAll(rls.Where(rl => isEmptyName || names.Contains(rl.PrimaryKey))
                        .Select(rl => AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
                        {
                            ApplyResource(target, position, rotation, name, resource, type, callback);
                        })));
                }
            }
        }

        /// <summary>
        /// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        /// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        /// object will be a stand alone object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="names"></param>
        /// <param name="label"></param>
        /// <param name="callback"></param>
        public async Task Apply<T>(GameObject target, Slot slot, Vector3 position, Quaternion rotation, List<string> names = null, string label = null, ResourceLoadedCallback<T> callback = null)
        {

            target = FindSlotTarget(target, slot) ?? target;

            bool isEmptyName = StringUtil.IsListEmpty(names);
            if (isEmptyName && string.IsNullOrEmpty(label))
            {
                Debug.LogError("Names and Label must have at least one piece of valid data");
            }
            else
            {

                if (string.IsNullOrEmpty(label))
                {

                    //Label is null
                    foreach (string key in names)
                    {
                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsAsync(key);
                        Dictionary<string, Task<T>> tasks = new Dictionary<string, Task<T>>();
                        foreach (var rl in rls)
                        {
                            if (rl.ResourceType == typeof(T))
                            {
                                //Load resources
                                tasks.Add(rl.PrimaryKey, AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl));
                            }
                        }
                        //Wait for completion
                        await Task.WhenAll(tasks.Values);
                        foreach (var pair in tasks)
                        {
                            //Apply
                            ApplyResource<T>(target, position, rotation, pair.Key, await pair.Value, callback);
                        }
                    }
                }
                else
                {
                    //Apply resources under same label as a batch
                    var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(label);
                    Dictionary<string, Task<T>> tasks = new Dictionary<string, Task<T>>();
                    foreach (var rl in rls)
                    {
                        if (rl.ResourceType == typeof(T) && (isEmptyName || names.Contains(rl.PrimaryKey)))
                        {
                            //Load resources
                            tasks.Add(rl.PrimaryKey, AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl));
                        }
                    }
                    //Wait for completion
                    await Task.WhenAll(tasks.Values);
                    foreach (var pair in tasks)
                    {
                        //Apply
                        ApplyResource<T>(target, position, rotation, pair.Key, await pair.Value, callback);
                    }
                }
            }
        }

        /// <summary>
        /// Find the target GameObject matches the specified slot
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        private GameObject FindSlotTarget(GameObject gameObject, Slot slot)
        {

            if (gameObject != null && slot != Slot.All && slot != Slot.None && slot != Slot.Root)
            {

                EquipmentSlot[] eSlots = gameObject.GetComponentsInChildren<EquipmentSlot>();
                foreach (var eSlot in eSlots)
                {
                    if (eSlot.Slot == slot)
                    {
                        return eSlot.gameObject;
                    }
                }
            }
            return null;
        }
    }
}