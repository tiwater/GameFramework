using UnityEngine;
using System.Collections;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;

namespace GameFramework.GameStructure.AddressableGameItems.Components
{
    /// <summary>
    /// Handle the display of AddressableGameItem and hold the PlayerGameItem instance of it
    /// </summary>
    /// Hold the Character GameItem, mount the equiment, textures, etc. And display it when the assets are ready
    [AddComponentMenu("Game Framework/GameStructure/AddressableGameItems/AddressableGameItem Holder")]
    public class AddressableGameItemHolder : GameItemInstanceHolder<AddressableGameItem>
    {
        protected override GameItemManager<AddressableGameItem, GameItem> GetGameItemManager()
        {
            return GameManager.Instance.AddressableGameItems;
        }
    }
}
