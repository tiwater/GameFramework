using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace GameFramework.Service
{
    public class AddressableResService
    {
        public delegate void ResourceLocationHandler(IResourceLocation rl);
        public delegate void ResourceLoadedCallback<T>(string name, T resource);
        public delegate void ResourceLoadedCallback(string name, object resource, Type ResourceType);
        public delegate void SingleResourceLoadedCallback<T>(T resource);

        private List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();
        private Dictionary<string, object> resources = new Dictionary<string, object>();
        private Dictionary<string, IList<IResourceLocation>> resourcesLocations = new Dictionary<string, IList<IResourceLocation>>();

        public List<string> preloadResourceLabels;

        private static AddressableResService _instance = new AddressableResService();
        public static AddressableResService GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Load the required resources
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="names"></param>
        /// <param name="label"></param>
        /// <param name="resourceLoadedCallback">If loaded scene, the callback is ignored</param>
        public async Task<List<T>> LoadResourcesAsync<T>(List<string> names, string label)
        {
            List<T> resources = new List<T>();
            if (label == null)
            {
                //hasn't label
                foreach (string name in names)
                {
                    //Load each resource
                    T resource = await LoadResourceAsync<T>(name);
                    if (resource != null)
                    {
                        resources.Add(resource);
                    }
                }
            }
            else
            {
                //Get the ResourceLocations for the label first
                var rls = await LoadResourceLocationsForLabelAsync(label);
                var resourceSelector = rls.Where(rl => (names == null || names.Count() == 0 || names.Contains(rl.PrimaryKey)))
                    .Select(rl => LoadResourceByLocationAsync<T>(rl));

                await Task.WhenAll(resourceSelector);
                foreach (var result in resourceSelector)
                {
                    var res = await result;
                    if (res != null)
                    {
                        resources.Add(res);
                    }
                }
            }
            return resources;
        }

        /// <summary>
        /// Load the resources under given label
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <param name="resourceLoadedCallback"></param>
        public async Task<List<T>> LoadResourcesAsync<T>(string label)
        {
            return await LoadResourcesAsync<T>(null, label);
        }


        /// <summary>
        /// Load ResourceLocation for specified resource
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rlHandler"></param>
        public async Task<IList<IResourceLocation>> LoadResourceLocationsAsync(string key)
        {
            return await LoadResourceLocationsAsync(key, null);
        }

        /// <summary>
        /// Load ResourceLocation for specified resource
        /// </summary>
        /// <param name="key"></param>
        /// <param name="label"></param>
        /// <param name="rlHandler"></param>
        public async Task<IList<IResourceLocation>> LoadResourceLocationsAsync(string key, string label)
        {
            Assert.IsNotNull(key, "Key cannot be null");
            List<string> keys;
            if (label == null)
            {
                keys = new List<string> { key };
            }
            else
            {
                keys = new List<string> { key, label };
            }
            var handle = Addressables.LoadResourceLocationsAsync(keys,
                Addressables.MergeMode.Intersection);
            handles.Add(handle);
            await handle;
            return handle.Result;
        }

        /// <summary>
        /// Get ResourceLocations for specified addressable label. If want to handle mutiple asset types in one call,
        /// use this API
        /// </summary>
        /// <param name="label"></param>
        /// <param name="rlHandler"></param>
        public async Task<IList<IResourceLocation>> LoadResourceLocationsForLabelAsync(string label)
        {
            //If already have resource location
            if (resourcesLocations.ContainsKey(label))
            {
                //Just use it
                return resourcesLocations[label];
            }
            else
            {
                //Otherwise load the resource location
                var handle = Addressables.LoadResourceLocationsAsync(label);
                handles.Add(handle);
                await handle;
                resourcesLocations[label] = handle.Result;
                return handle.Result;
            }
        }

        /// <summary>
        /// Load a single addressable resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="label"></param>
        /// <param name="resourceLoadedCallback"></param>
        public async Task<T> LoadResourceAsync<T>(string name, string label)
        {

            //LoadResources<T>(new List<string> { key }, label, (key, resource) => resourceLoadedCallback(resource));
            if (label == null)
            {
                return await LoadResourceAsync<T>(name);
            }
            else if (name == null)
            {
                Assert.IsNotNull(name, "Name shouldn't be null");
                return default(T);
            }
            else
            {
                string storeKey = name + "_" + label + typeof(T).ToString();
                //If we already loaded the resource
                if (resources.ContainsKey(storeKey))
                {
                    //Use it directly
                    return (T)resources[storeKey];
                }
                else
                {
                    var handle = Addressables.LoadResourceLocationsAsync(new List<object> { name, label },
                            Addressables.MergeMode.Intersection);
                    handles.Add(handle);
                    await handle;

                    var resourceSelector = handle.Result.Select(rl => LoadResourceByLocationAsync<T>(rl));
                    await Task.WhenAll(resourceSelector);
                    foreach (var result in resourceSelector)
                    {
                        var resource = await result;
                        resources[storeKey] = resource;
                        return resource;
                    }
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Load the specified addressable resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="resourceLoadedCallback"></param>
        public async Task<T> LoadResourceAsync<T>(string name)
        {

            //If we already loaded the resource
            if (resources.ContainsKey(name + typeof(T).ToString()))
            {
                //Use it directly
                return (T)resources[name];
            }
            else
            {
                //Load it
                if (typeof(T) == typeof(SceneInstance))
                {
                    //If it is scene, load it to switch current scene
                    handles.Add(Addressables.LoadSceneAsync(name));
                    T result = default(T);
                    return result;
                }
                else
                {
                    var handle = Addressables.LoadAssetAsync<T>(name);
                    handles.Add(handle);
                    await handle;
                    var resource = handle.Result;
                    resources[name + typeof(T).ToString()] = resource;
                    return resource;
                }
            }
        }

        /// <summary>
        /// Load Resource by IResourceLocation, and pass back the resourceType
        /// </summary>
        /// <param name="rl"></param>
        /// <param name="callback"></param>
        public async Task LoadResourceByLocationAsync(IResourceLocation rl, ResourceLoadedCallback callback)
        {
            if (rl.ResourceType == typeof(SceneInstance))
            {
                //Scene
                GetInstance().LoadResourceByLocationAsync<SceneInstance>(rl);
            }
            else if (rl.ResourceType == typeof(Texture2D))
            {
                //Texture
                callback(rl.PrimaryKey, await GetInstance().LoadResourceByLocationAsync<Texture2D>(rl), rl.ResourceType);
            }
            else if (rl.ResourceType == typeof(GameObject))
            {
                //Prefab
                callback(rl.PrimaryKey, await GetInstance().LoadResourceByLocationAsync<GameObject>(rl), rl.ResourceType);
            }
            else
            {
                Debug.LogWarning(string.Format("Unsupported type {0} of resource {1} at {2}, please add the code to handle it!!!",
                    rl.ResourceType, rl.PrimaryKey, rl.InternalId));
            }
        }

        /// <summary>
        /// Load scene to replace current scene
        /// </summary>
        /// <param name="rl"></param>
        public void LoadSceneAsync(IResourceLocation rl)
        {
            Assert.AreEqual(typeof(SceneInstance), rl.ResourceType, "The resource types are not match");
            handles.Add(Addressables.LoadSceneAsync(rl));
            return;
        }

        /// <summary>
        /// Load resource by the ResourceLocation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rl"></param>
        /// <param name="resourceLoadedCallback"></param>
        public async Task<T> LoadResourceByLocationAsync<T>(IResourceLocation rl)
        {
            if (typeof(T) == typeof(SceneInstance))
            {
                //If it is scene, load it to switch current scene
                LoadSceneAsync(rl);
                T result = default(T);
                return result;
            }
            else
            {
                if (resources.ContainsKey(rl.InternalId))
                {
                    //If already loaded, use it directly
                    return (T)resources[rl.InternalId];
                }
                else
                {
                    //Load it
                    var handle = Addressables.LoadAssetAsync<T>(rl);
                    handles.Add(handle);
                    await handle;

                    resources[rl.InternalId] = handle.Result;
                    return handle.Result;
                }
            }
        }

        public void Clear()
        {
            Debug.Log("Clear resources");
            //foreach ( var handle in handles)
            //{
            //    Addressables.ReleaseInstance(handle);
            //}
            //handles.Clear();
            //resourcesLocations.Clear();
            //resources.Clear();
        }

    }
}