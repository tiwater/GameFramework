using UnityEngine;
using System.Collections;
using GameFramework.GameStructure.Levels.Components;
using GameFramework.GameStructure.Service;

namespace GameFramework.GameStructure.AddressableGameItems.Components
{
    /// <summary>
    /// Handle the addressables handles release when switch scenes
    /// </summary>
    /// Add this to SceneManager to release addressable handles.
    [AddComponentMenu("Game Framework/GameStructure/AddressableGameItem/Scene Addressable Resource Manager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class SceneAddressableResourceManager : MonoBehaviour
    {

        private void OnDestroy()
        {
            AddressableResService.GetInstance().Clear();
        }
    }
}