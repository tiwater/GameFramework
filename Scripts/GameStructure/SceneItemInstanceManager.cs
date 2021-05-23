using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.Characters;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using GameFramework.GameStructure.AddressableGameItems.Components;
using GameFramework.GameStructure.Util;
using GameFramework.Platform.Android;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace GameFramework.GameStructure
{
    /// <summary>
    /// A core component that holds and manages information about a scene.
    /// </summary>
    /// GameManager is where you can setup the structure of your game and holdes other key information and functionality relating to Preferences,
    /// GameStructure, Display, Localisation, Audio, Messaging and more. Please see the online help for full information.
    [AddComponentMenu("Game Framework/GameStructure/Scene Item Instance Manager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class SceneItemInstanceManager : MonoBehaviour
    {
        public GameObject LevelHolder;
        public GameObject CharacterHolder;
        public GameObject AgiHolder;

        public GameObject PlayerCharacterHolder { get; set; }

        private void Start()
        {
            StartCoroutine(LateStart());
        }

        IEnumerator LateStart()
        {
            while (!GameManager.Instance.IsInitialised)
            {
                yield return Task.Yield();
            }
            DisplayPlayGameItems(transform, GameManager.Instance.SceneRootNode);
        }

        private void DisplayPlayGameItems(Transform parent, PlayerGameItem item)
        {
            //Get the position and rotation info for the GameItem
            var positionVariable = item.ExtraProps.GetVector3(Constants.PROP_KEY_POSITION);
            var rotationVariable = item.ExtraProps.GetVector3(Constants.PROP_KEY_ROTATION);
            Vector3 position;
            Quaternion rotation;
            //Position
            if (positionVariable != null)
            {
                position = positionVariable.Value;
            }
            else
            {
                position = Vector3.zero;
            }
            //Rotation
            if (rotationVariable != null)
            {
                rotation = Quaternion.Euler(rotationVariable.Value);
            }
            else
            {
                rotation = Quaternion.identity;
            }
            GameObject gameObject = null;

            if (item.GiType == typeof(Level).Name)
            {
                //Level
                gameObject = Instantiate(LevelHolder,
                    position, rotation, parent);
            }
            else if (item.GiType == typeof(Character).Name)
            {
                //Create the character holder
                gameObject = Instantiate(CharacterHolder,
                    position, rotation, parent);
                //Config the prefab
                var holderComponent = gameObject.GetComponent<CharacterHolder>();
                holderComponent.BindCharacterPGI(item, item.PrefabType);
                if (item.IsActive)
                {
                    //Link to the player's character holder
                    PlayerCharacterHolder = gameObject;
                }
                //holderComponent.Context.ContextMode = GameItems.ObjectModel.GameItemContext.ContextModeType.ByNumber;
                //holderComponent.Context.Number = item.GiId;
                //holderComponent.PrefabType = item.PrefabType;
                //holderComponent.PlayerGameItem = item;
            }
            else if (item.GiType == typeof(AddressableGameItem).Name)
            {
                AddressableGameItem gameItem = GameManager.Instance.AddressableGameItems.GetItem(item.GiId);
                //Won't create GameObject for Skin (Texture).
                if (gameItem.ContentType != Model.AddressableGameItemMeta.ContentType.Skin)
                {
                    //AddressableGameItem
                    gameObject = Instantiate(AgiHolder,
                        position, rotation, parent);
                    //Config the prefab
                    var holderComponent = gameObject.GetComponent<AddressableGameItemHolder>();
                    holderComponent.Context.ContextMode = GameItems.ObjectModel.GameItemContext.ContextModeType.ByNumber;
                    holderComponent.Context.Number = item.GiId;
                    holderComponent.PrefabType = item.PrefabType;
                }
            }
            if (item.Children != null && gameObject != null)
            {
                //Handle the children's display
                foreach (var child in item.Children)
                {
                    DisplayPlayGameItems(gameObject.transform, child);
                }
            }
        }

        private void OnDestroy()
        {
        }
    }
}