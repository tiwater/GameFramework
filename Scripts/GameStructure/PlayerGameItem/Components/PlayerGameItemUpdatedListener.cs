using System;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.PlayerGameItems.Messages;
using GameFramework.GameStructure.Util;
using GameFramework.Messaging.Components.AbstractClasses;

namespace GameFramework.GameStructure.PlayerGameItems.Components
{
    /// <summary>
    /// Listen to the PlayerGameItemUpdatedMessage, default behavior is to copy the properties to the original item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="M"></typeparam>
    public class PlayerGameItemUpdatedListener<T, M> : RunOnMessage<PlayerGameItemUpdatedMessage> where T : GameItemInstanceHolder<M> where M : GameItem
    {
        protected T itemHolder;
        public override void Start()
        {
            base.Start();
            itemHolder = GetComponent<T>();
        }

        public override bool RunMethod(PlayerGameItemUpdatedMessage message)
        {
            if (itemHolder != null && message.OldItem.Id == itemHolder.PlayerGameItem.Id)
            {
                //It's me to update
                var target = GameManager.Instance.PlayerGameItems.GetPlayerGameItemById(itemHolder.PlayerGameItem.Id);
                //Copy the value for the entity
                ObjectUtil.CopyObject(target, message.NewItem);
                target.Props = message.NewItem.Props;
                target.ExtraProps = message.OldItem.ExtraProps;
            }

            return true;
        }
    }
}