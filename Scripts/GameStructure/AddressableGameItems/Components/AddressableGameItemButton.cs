using GameFramework.Billing.Messages;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Messaging;
using UnityEngine;

namespace GameFramework.GameStructure.AddressableGameItems.Components
{
    /// <summary>
    /// AddressableGameItem Details Button
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/AddressableGameItem/AddressableGameItemButton")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class AddressableGameItemButton : GameItemButton<AddressableGameItem>
    {
        /// <summary>
        /// Pass static parametres to base class.
        /// </summary>
        public AddressableGameItemButton() : base("AddressableGameItem") { }

        protected override void Awake()
        {
            base.Awake();
            GameManager.SafeAddListener<AddressableGameItemPurchasedMessage>(AddressableGameItemPurchasedHandler);
        }

        //protected override void Start()
        //{
        //    base.Start();
        //    //If has no sprite on button
        //    if (DisplayImage.sprite == null)
        //    {
        //        //Try to set a sprite
        //        AddressableGameItem gameItem = GetGameItem<AddressableGameItem>();
        //        if (gameItem.Sprite != null)
        //        {
        //            //Get from AddressableGameItem if it has sprite
        //            DisplayImage.sprite = gameItem.Sprite;
        //        }
        //        else
        //        {
        //            //Otherwise try to load the addressable resource
        //            if (gameItem.Thumbnail != null)
        //            {
        //                AddressableResService.GetInstance().LoadResourceAsync<Sprite>(gameItem.Thumbnail, sprite =>
        //                {
        //                    DisplayImage.sprite = sprite;
        //                });
        //            }
        //        }
        //    }
        //}

        protected override void OnDestroy()
        {
            GameManager.SafeRemoveListener<AddressableGameItemPurchasedMessage>(AddressableGameItemPurchasedHandler);
            base.OnDestroy();
        }


        /// <summary>
        /// Handler for AddressableGameItem purchase messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool AddressableGameItemPurchasedHandler(BaseMessage message)
        {
            var AddressableGameItemPurchasedMessage = message as AddressableGameItemPurchasedMessage;
            UnlockIfNumberMatches(AddressableGameItemPurchasedMessage.Number);
            return true;
        }


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