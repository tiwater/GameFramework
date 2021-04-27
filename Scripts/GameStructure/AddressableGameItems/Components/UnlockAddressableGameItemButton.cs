//----------------------------------------------
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;

namespace GameFramework.GameStructure.AddressableGameItems.Components
{
    /// <summary>
    /// Unlock GameItem button for AddressableGameItems 
    /// </summary>
    /// Add this to a UI button for automatic handling of unlocking AddressableGameItems.
    [AddComponentMenu("Game Framework/GameStructure/AddressableGameItem/Unlock AddressableGameItem Button")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class UnlockAddressableGameItemButton : UnlockGameItemButton<AddressableGameItem>
    {
        /// <summary>
        /// Pass static parametres to base class.
        /// </summary>
        public UnlockAddressableGameItemButton() : base("AddressableGameItem") { }

        /// <summary>
        /// Returns the GameItemsMaager that holds AddressableGameItems
        /// </summary>
        /// <returns></returns>
        protected override GameItemManager<AddressableGameItem, GameItem> GetGameItemManager()
        {
            return GameManager.Instance.AddressableGameItems;
        }
    }
}