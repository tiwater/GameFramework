using UnityEngine;
using System.Collections;
using GameFramework.Platform.Messaging;
using GameFramework.GameStructure.Platform.Messaging;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure;
using GameFramework.GameStructure.PlayerGameItems.Messages;
using GameFramework.GameStructure.Util;
using System.Collections.Generic;

namespace GameFramework.GameStructure.PlayerGameItems.Components
{
    /// <summary>
    /// Convert the PlayerGameItem related PlatformMessage to Specific update message and dispatch it
    /// </summary>
    public class UpdateMessageDispatcher : PlatformMessageDispatcher
    {
        public override void OnMessage(PlatformMessage message)
        {
            //Convert the PlatformMessage to specific message or handle it directly
            if (message.Content.ContainsKey(PlatformMessage.PLATFORM_MESSAGE_TYPE_KEY))
            {
                string type = message.Content[PlatformMessage.PLATFORM_MESSAGE_TYPE_KEY] as string;
                if (type == PlatformMessage.PLATFORM_MESSAGE_TYPE_CREATION_UPDATED)
                {
                    //Creation properties updated
                    PlayerGameItem newItem = JsonUtility.FromJson<PlayerGameItem>((string)message.Content[PlatformMessage.PLATFORM_MESSAGE_CONTENT_PGI_KEY]);
                    PlayerGameItem oldItem = GameManager.Instance.PlayerGameItems.GetPlayerGameItemById(newItem.Id);
                    PlayerGameItemUpdatedMessage pgiMessage = new PlayerGameItemUpdatedMessage(oldItem, newItem);

                    GameManager.SafeQueueMessage(pgiMessage);
                    return;
                }
                else if (type == PlatformMessage.PLATFORM_MESSAGE_TYPE_EQUIPMENT_UPDATED)
                {
                    //Equipment updated
                    List<PlayerGameItem> newEquipments = JsonUtil.ListFromJson<PlayerGameItem>((string)message.Content[PlatformMessage.PLATFORM_MESSAGE_CONTENT_EQUIPMENT_KEY]);
                    List<PlayerGameItem> oldEquipments = GameManager.Instance.PlayerGameItems.SelectedCharacter.Equipments;
                    string charId = (string)message.Content[PlatformMessage.PLATFORM_MESSAGE_CHARACTER_ID_KEY];
                    EquipmentUpdatedMessage eqMessage = new EquipmentUpdatedMessage(charId, oldEquipments, newEquipments);

                    GameManager.SafeQueueMessage(eqMessage);
                    return;
                }
            }
            //Not handled, queue the message
            base.OnMessage(message);
        }
    }
}