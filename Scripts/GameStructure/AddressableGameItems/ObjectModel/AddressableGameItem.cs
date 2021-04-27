using UnityEngine;
using GameFramework.GameStructure.GameItems.ObjectModel;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using static GameFramework.GameStructure.Model.GameItemEquipment;
using GameFramework.GameStructure.Model;
using static GameFramework.GameStructure.Service.AddressableResService;
using GameFramework.GameStructure.Service;
using GameFramework.GameStructure.Characters.ObjectModel;

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
        public List<Slot> Slots = new List<Slot>();

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
        /// The GameItem this character supports. Store in <GameItemType, GameItemId> format,
        /// <GameItemType, null> means support all GameItems under the given type.
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
        /// The addressable resources the GameItem exported. Store in <name, label> format.
        /// The combination of name and label should be able to locate the unique resource.
        /// </summary>
        public Dictionary<string, string> Resources
        {
            get
            {
                return _resources;
            }
        }
        Dictionary<string, string> _resources = new Dictionary<string, string>();

        private string NULL_LABEL = "NULL_LABEL";

        /// <summary>
        /// Group the keys by label
        /// </summary>
        /// <param name="names"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> filterResourceList(List<string> names, string label)
        {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();

            if (Resources != null && Resources.Count > 0)
            {
                //Has limited resources
                foreach (var content in Resources)
                {
                    if (((names == null || names.Contains(content.Key)) && label == content.Value) ||
                        (names == null && label == null))
                    {
                        List<string> keys;
                        string keyLabel = content.Value ?? NULL_LABEL;
                        if (res.ContainsKey(keyLabel))
                        {
                            keys = res[keyLabel];
                        }
                        else
                        {
                            keys = new List<string>();
                            res[keyLabel] = keys;
                        }
                        keys.Add(content.Key);
                    }
                }
            }
            else
            {
                if ((names == null || names.Count == 0) && label == null)
                {
                    //No limitation? Then default limit to package
                    res[Package] = null;
                    return res;
                }
                //Limited by the input field
                string keyLabel = label ?? NULL_LABEL;
                res[keyLabel] = names;
            }
            return res;

        }

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
            Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
            target = FindSlotTarget(target, slot) ?? target;
            if (applyResource.Count > 0)
            {
                //Only apply the specified resources
                foreach (var content in applyResource)
                {
                    if (content.Key == NULL_LABEL)
                    {
                        //Label is null
                        foreach (string key in content.Value)
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
                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key);
                        await Task.WhenAll(rls.Where(rl => (content.Value == null || content.Value.Count() == 0 || content.Value.Contains(rl.PrimaryKey)))
                            .Select(rl => AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
                        {
                            ApplyResource(target, name, resource, type, instantiateInWorldSpace, callback);
                        })));
                    }
                }
            }
            else
            {
                //No resources are allow to apply
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
            Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
            target = FindSlotTarget(target, slot) ?? target;
            if (applyResource.Count > 0)
            {
                //Only apply the specified resources
                foreach (var content in applyResource)
                {
                    if (content.Key == NULL_LABEL)
                    {
                        //Label is null
                        foreach (string key in content.Value)
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
                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key);
                        Dictionary<string, Task<T>> tasks = new Dictionary<string, Task<T>>();
                        foreach (var rl in rls)
                        {
                            if (rl.ResourceType == typeof(T) && content.Value.Contains(rl.PrimaryKey))
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
            else
            {
                //No resources are allow to apply
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
            Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
            target = FindSlotTarget(target, slot) ?? target;
            if (applyResource.Count > 0)
            {
                //Only apply the specified resources
                foreach (var content in applyResource)
                {
                    if (content.Key == NULL_LABEL)
                    {
                        //Label is null
                        foreach (string key in content.Value)
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
                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key);
                        await Task.WhenAll(rls.Where(rl => content.Value.Contains(rl.PrimaryKey))
                            .Select(rl => AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
                            {
                                ApplyResource(target, position, rotation, name, resource, type, callback);
                            })));
                    }
                }
            }
            else
            {
                //No resources are allow to apply
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

            Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
            target = FindSlotTarget(target, slot) ?? target;
            if (applyResource.Count > 0)
            {
                //Only apply the specified resources
                foreach (var content in applyResource)
                {
                    if (content.Key == NULL_LABEL)
                    {
                        //Label is null
                        foreach (string key in content.Value)
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
                        var rls = await AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key);
                        Dictionary<string, Task<T>> tasks = new Dictionary<string, Task<T>>();
                        foreach (var rl in rls)
                        {
                            if (rl.ResourceType == typeof(T) && content.Value.Contains(rl.PrimaryKey))
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
            else
            {
                //No resources are allow to apply
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








        //==========================

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
        //public void Apply(GameObject target, List<string> names = null, string label = null, bool instantiateInWorldSpace = false, ResourceLoadedCallback callback = null)
        //{
        //    Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
        //    if (applyResource.Count > 0)
        //    {
        //        //Only apply the specified resources
        //        foreach (var content in applyResource)
        //        {
        //            if (content.Key == NULL_LABEL)
        //            {
        //                //Label is null
        //                foreach (string key in content.Value)
        //                {
        //                    AddressableResService.GetInstance().LoadResourceLocationsAsync(key, rl =>
        //                    {
        //                        AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
        //                        {
        //                            ApplyResource(target, name, resource, type, instantiateInWorldSpace, callback);
        //                        });
        //                    });
        //                }
        //            }
        //            else
        //            {

        //                //Apply resources under same label as a batch
        //                AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key, rl =>
        //                {
        //                    AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) => {
        //                        ApplyResource(target, name, resource, type, instantiateInWorldSpace, callback);
        //                    });
        //                });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //No resources are allow to apply
        //    }
        //}

        ///// <summary>
        ///// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        ///// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        ///// object will be a stand alone object
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="target"></param>
        ///// <param name="names"></param>
        ///// <param name="label"></param>
        ///// <param name="instantiateInWorldSpace"></param>
        ///// <param name="callback"></param>
        //public async Task Apply<T>(GameObject target, List<string> names = null, string label = null, bool instantiateInWorldSpace = false, ResourceLoadedCallback<T> callback = null)
        //{
        //    Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
        //    if (applyResource.Count > 0)
        //    {
        //        //Only apply the specified resources
        //        foreach (var content in applyResource)
        //        {
        //            if (content.Key == NULL_LABEL)
        //            {
        //                //Label is null
        //                foreach (string key in content.Value)
        //                {
        //                    var rls = await AddressableResService.GetInstance().LoadResourceLocationsAsync(key);
        //                    foreach (var rl in rls)
        //                    {
        //                        if (rl.ResourceType == typeof(T))
        //                        {
        //                            var resource = await AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl);
        //                            ApplyResource<T>(target, rl.PrimaryKey, resource, instantiateInWorldSpace, callback);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {

        //                //Apply resources under same label as a batch

        //                var rls = await AddressableResService.GetInstance().LoadResourceLocationsAsync(content.Key);
        //                foreach (var rl in rls)
        //                {
        //                    if (rl.ResourceType == typeof(T))
        //                    {
        //                        var resource = await AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl);
        //                        ApplyResource<T>(target, rl.PrimaryKey, resource, instantiateInWorldSpace, callback);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //No resources are allow to apply
        //    }
        //}

        ///// <summary>
        ///// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        ///// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        ///// object will be a stand alone object
        ///// </summary>
        ///// <param name="target"></param>
        ///// <param name="position"></param>
        ///// <param name="rotation"></param>
        ///// <param name="names"></param>
        ///// <param name="label"></param>
        ///// <param name="callback"></param>
        //public void Apply(GameObject target, Vector3 position, Quaternion rotation, List<string> names = null, string label = null, ResourceLoadedCallback callback = null)
        //{
        //    Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
        //    if (applyResource.Count > 0)
        //    {
        //        //Only apply the specified resources
        //        foreach (var content in applyResource)
        //        {
        //            if (content.Key == NULL_LABEL)
        //            {
        //                //Label is null
        //                foreach (string key in content.Value)
        //                {
        //                    AddressableResService.GetInstance().LoadResourceLocationsAsync(key, rl =>
        //                    {
        //                        AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) =>
        //                        {
        //                            ApplyResource(target, position, rotation, name, resource, type, callback);
        //                        });
        //                    });
        //                }
        //            }
        //            else
        //            {

        //                //Apply resources under same label as a batch
        //                AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key, rl =>
        //                {
        //                    AddressableResService.GetInstance().LoadResourceByLocationAsync(rl, (name, resource, type) => {
        //                        ApplyResource(target, position, rotation, name, resource, type, callback);
        //                    });
        //                });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //No resources are allow to apply
        //    }
        //}

        ///// <summary>
        ///// Apply the resources in the AddressableGameItem to the target GameObject. If the resource is texture, will try to replace the texture with same name.
        ///// If the resource is prefab, will be instantiated as a child object under the given target. If the target is null, then the instantiated
        ///// object will be a stand alone object
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="target"></param>
        ///// <param name="position"></param>
        ///// <param name="rotation"></param>
        ///// <param name="names"></param>
        ///// <param name="label"></param>
        ///// <param name="callback"></param>
        //public void Apply<T>(GameObject target, Vector3 position, Quaternion rotation, List<string> names = null, string label = null, ResourceLoadedCallback<T> callback = null)
        //{

        //    Dictionary<string, List<string>> applyResource = filterResourceList(names, label);
        //    if (applyResource.Count > 0)
        //    {
        //        //Only apply the specified resources
        //        foreach (var content in applyResource)
        //        {
        //            if (content.Key == NULL_LABEL)
        //            {
        //                //Label is null
        //                foreach (string key in content.Value)
        //                {
        //                    AddressableResService.GetInstance().LoadResourceLocationAsync(key, rl =>
        //                    {
        //                        if (rl.ResourceType == typeof(T))
        //                        {
        //                            AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl, (resource) =>
        //                            {
        //                                ApplyResource<T>(target, position, rotation, name, resource, callback);
        //                            });
        //                        }
        //                    });
        //                }
        //            }
        //            else
        //            {

        //                //Apply resources under same label as a batch
        //                AddressableResService.GetInstance().LoadResourceLocationsForLabelAsync(content.Key, rl =>
        //                {
        //                    if (rl.ResourceType == typeof(T))
        //                    {
        //                        AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl, (resource) =>
        //                        {
        //                            AddressableResService.GetInstance().LoadResourceByLocationAsync<T>(rl, (resource) =>
        //                            {
        //                                ApplyResource<T>(target, position, rotation, name, resource, callback);
        //                            });
        //                        });
        //                    }
        //                });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //No resources are allow to apply
        //    }
        //}
    }
}