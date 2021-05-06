using System;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.Characters.Components
{
    /// <summary>
    /// Click on the Character button to create a new instance for the player
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Characters/CharacterInstanceButton")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/characters/")]
    public class CharacterInstanceButton : CharacterButton
    {

        /// <summary>
        /// Called when an unlocked button is clicked
        /// </summary>
        /// The default implementation sets the GameItemManager's selected item and then if specified loads the scene specified by ClickUnlockedSceneToLoad.
        /// You may override this in a derived class.
        public async override void ClickUnlocked()
        {
            var character = GetGameItem<Character>();
            var instance = character.GenerateGameItemInstance();
            instance.IsActive = true;
            await GameManager.Instance.Player.PlayerGameItem.AddChild(instance);

            if (SelectionMode == GameItemButton.SelectionModeType.ClickThrough && !string.IsNullOrEmpty(ClickUnlockedSceneToLoad))
            {
                GameManager.LoadSceneWithTransitions(string.Format(ClickUnlockedSceneToLoad, GameItem.GiId));
            }
        }
    }
}