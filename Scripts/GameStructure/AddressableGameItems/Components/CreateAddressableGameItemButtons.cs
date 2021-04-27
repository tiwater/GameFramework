using System.Collections.Generic;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.Messages;
using UnityEngine;
using static GameFramework.GameStructure.Model.AddressableGameItemMeta;

namespace GameFramework.GameStructure.AddressableGameItems.Components
{
    /// <summary>
    /// Creates button instances for all AddressableGameItems using a referenced prefab.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/AddressableGameItem/Create AddressableGameItem Buttons")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class CreateAddressableGameItemButtons : CreateGameItemButtons<AddressableGameItemButton, AddressableGameItem>
    {

        /// <summary>
        /// The GameItemType of the AddressableGameItemButtons to create.
        /// </summary>
        [Tooltip("The GameItemType of the AddressableGameItemButtons to create.")]
        public ContentType gameItemType;

        Stack<GameObject> _buttonGameObjects = new Stack<GameObject>();

        /// <summary>
        /// Return a GameItemManager that this works upon.
        /// </summary>
        /// <returns></returns>
        protected override GameItemManager<AddressableGameItem, GameItem> GetGameItemManager()
        {
            return GameManager.Instance.AddressableGameItems;
        }

        #region Unity Lifecycle

        /// <summary>
        /// Create and add all buttons
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // react to changes to world selection to recreate buttons
            GameManager.Messenger.AddListener<PlayerGameItemLoadedMessage>(OnMessageCreateButtons);
        }


        /// <summary>
        /// Unsubscribe from world change messages 
        /// </summary>        
        protected override void OnDestroy()
        {
            if (GameManager.IsActive)
                GameManager.Messenger.RemoveListener<PlayerGameItemLoadedMessage>(OnMessageCreateButtons);
        }

        #endregion Unity Lifecycle


        protected override void CreateButtons()
        {
            Debug.Log("Customized CreateButtons");
            // first delete any old buttons
            while (_buttonGameObjects.Count != 0)
            {
                var buttonGameObject = _buttonGameObjects.Pop();
                Destroy(buttonGameObject);
            }

            var button = Prefab.GetComponent<AddressableGameItemButton>();
#if UNITY_EDITOR
            // prefab values will get overwritten if running in editor mode so save and restore.
            var oldContext = button.Context.ContextMode;
            var oldSelectionMode = button.SelectionMode;
            var oldScene = button.ClickUnlockedSceneToLoad;
            var oldClickToUnlock = button.ClickToUnlock;
#endif
            button.Context.ContextMode = GameItemContext.ContextModeType.Reference;
            button.SelectionMode = SelectionMode;
            button.ClickUnlockedSceneToLoad = ClickUnlockedSceneToLoad;
            button.ClickToUnlock = ClickToUnlock;

            //If not categorized, then display all items
#pragma warning disable 0168
            foreach (var gameItem in GetGameItemManager())
#pragma warning restore 0168
            {
                if (gameItemType == ContentType.All || gameItemType == gameItem.ContentType)
                {
                    CreateButtonForItem(gameItem);
                }
            }

#if UNITY_EDITOR
            // prefab values will get overwritten if running in editor mode so save and restore.
            button.Context.ContextMode = oldContext;
            button.SelectionMode = oldSelectionMode;
            button.ClickUnlockedSceneToLoad = oldScene;
            button.ClickToUnlock = oldClickToUnlock;
#endif
        }

        private void CreateButtonForItem(AddressableGameItem item)
        {

            var newObject = Instantiate(Prefab);
            var buttonInstance = newObject.GetComponent<AddressableGameItemButton>();
            buttonInstance.GameItem = item;
            //Reference to myself
            buttonInstance.Context.ReferencedGameItemContextBase = buttonInstance;
            newObject.transform.SetParent(transform, false);
            _buttonGameObjects.Push(newObject);
        }
    }
}