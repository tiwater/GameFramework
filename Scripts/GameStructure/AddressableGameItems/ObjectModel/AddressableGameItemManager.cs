using GameFramework.GameStructure.GameItems.ObjectModel;
using System.Threading.Tasks;
using GameFramework.GameStructure.Service;

namespace GameFramework.GameStructure.AddressableGameItems.ObjectModel
{
    public class AddressableGameItemManager : GameItemManager<AddressableGameItem, GameItem>
    {

        /// <summary>
        /// The ExtGameItems
        /// </summary>
        //public Dictionary<AddressableGameItemMeta.ContentType, List<AddressableGameItem>> CategorizedAddressableGameItems
        //{
        //    get
        //    {
        //        return _categorizedAddressableGameItems;
        //    }
        //}
        //Dictionary<AddressableGameItemMeta.ContentType, List<AddressableGameItem>> _categorizedAddressableGameItems;

        //private bool preloadPending;
        //private bool preloaded;

        //GameItemMetaService metaService = new GameItemMetaService();
        PlayerGameItemService gameItemService = new PlayerGameItemService();
        /// <summary>
        /// Load addressable GameItems
        /// </summary>
        //public IEnumerator LoadGameItemMetas(GameItemMetasHandler handler)
        //{
        //    //TODO: Web/local data sync
        //    yield return metaService.GetGameItemMetas(itemMetas=> {
        //        if (handler != null)
        //        {
        //            //Invoke the callback
        //            handler(itemMetas);
        //        }
        //        GameManager.SafeQueueMessage(new GameItemMetaLoadedMessage());
        //    });
        //}

        ///// <summary>
        ///// Process the GameItemMetas, generate the AddressableGameItems for each meta
        ///// </summary>
        //private void HandleGameItemMetas(AddressableGameItemMeta[] gameItemMetas)
        //{
        //    // Group the GameItems by type
        //    Dictionary<AddressableGameItemMeta.GameItemType, List<AddressableGameItem>> cat_items = new Dictionary<AddressableGameItemMeta.GameItemType, List<AddressableGameItem>>();
        //    //Index by the id
        //    Dictionary<string, AddressableGameItem> items = new Dictionary<string, AddressableGameItem>();

        //    foreach (AddressableGameItemMeta meta in gameItemMetas)
        //    {

        //        AddressableGameItem addrItem = meta.ToAddressableGameItem();

        //        //Get the category list
        //        List<AddressableGameItem> itemList;
        //        if (cat_items.ContainsKey(meta.Type))
        //        {
        //            itemList = cat_items[meta.Type];
        //        }
        //        else
        //        {
        //            //If the category doesn't exist
        //            //Then create it
        //            itemList = new List<AddressableGameItem>();
        //            cat_items[meta.Type] = itemList;
        //        }
        //        itemList.Add(addrItem);
        //        items[meta.Id] = addrItem;
        //    }
        //    _categorizedAddressableGameItems = cat_items;
        //    _indexedGameItems = items;
        //    Items = new List<AddressableGameItem>(_indexedGameItems.Values);
        //}

        public async Task Load()
        {
            await LoadAddressableResourcesAsync();

        }
    }
}