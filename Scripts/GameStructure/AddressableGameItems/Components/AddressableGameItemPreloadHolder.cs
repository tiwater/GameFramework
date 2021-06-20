using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure.AddressableGameItems.Components;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Components
{
    /// <summary>
    /// For some special cases we need to load the addressable items directly
    /// to avoid waiting for the GameManager and other resources preloading
    /// </summary>
    public class AddressableGameItemPreloadHolder : AddressableGameItemHolder
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            if (Context.GetReferencedContextMode() == ObjectModel.GameItemContext.ContextModeType.Selected)
                GetGameItemManager().SelectedChanged += SelectedChanged;
            LoadResource();
        }

        private async Task LoadResource()
        {
            await GetGameItemManager().LoadAddressableResources(new List<string>() { this.Context.Number });
            RunMethod(true);
        }
    }
}