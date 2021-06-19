using UnityEngine;
using System.Collections;
using GameFramework.Platform.Messaging;
using GameFramework.GameStructure.Platform.Messaging;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.GameStructure;
using GameFramework.GameStructure.PlayerGameItems.Messages;
using GameFramework.GameStructure.Util;
using System.Collections.Generic;
using GameFramework.Service;
using System.Threading.Tasks;
using GameFramework.Messaging.Components.AbstractClasses;

namespace GameFramework.GameStructure.PlayerGameItems.Components
{
    /// <summary>
    /// Convert the PlayerGameItem related PlatformMessage to Specific update message and dispatch it
    /// </summary>
    public class UpdateMessageDispatcher : RunOnMessage<PlatformMessage>
    {

        public override bool RunMethod(PlatformMessage message)
        {
            HandlePlatformMessage(message);
            return true;
        }

        private async Task HandlePlatformMessage(PlatformMessage message)
        {

            //Convert the PlatformMessage to specific message or handle it directly
            if (message.Content.ContainsKey(PlatformMessage.TYPE))
            {
                string type = message.Content[PlatformMessage.TYPE] as string;
                if (type == PlatformMessage.TYPE_CREATION_UPDATED)
                {
                    //Creation properties updated
                    string itemId = (string)message.Content[PlatformMessage.CONTENT_PGI_ID];

                    PlayerGameItem newItem = await PlayerGameItemService.Instance.GetPlayerGameItem(itemId);
                    //Clone the oldItem to avoid some message receiver may change the origin item
                    PlayerGameItem oldItem = JsonUtility.FromJson<PlayerGameItem>(JsonUtility.ToJson(GameManager.Instance.PlayerGameItems.GetPlayerGameItemById(newItem.Id)));
                    PlayerGameItemUpdatedMessage pgiMessage = new PlayerGameItemUpdatedMessage(oldItem, newItem);

                    GameManager.SafeQueueMessage(pgiMessage);
                    return;
                }
                else if (type == PlatformMessage.TYPE_EQUIPMENT_UPDATED)
                {
                    //Equipment updated
                    string itemId = (string)message.Content[PlatformMessage.CONTENT_PGI_ID];

                    List<PlayerGameItem> newEquipments = await PlayerGameItemService.Instance.GetEquipments(itemId);
                    //Copy the item to avoid some message receiver may change the origin item
                    List<PlayerGameItem> oldEquipments = JsonUtil.ListFromJson< PlayerGameItem >(JsonUtil.ListToJson<PlayerGameItem>(GameManager.Instance.PlayerGameItems.SelectedCharacter.Equipments));

                    EquipmentUpdatedMessage eqMessage = new EquipmentUpdatedMessage(itemId, oldEquipments, newEquipments);

                    GameManager.SafeQueueMessage(eqMessage);
                    return;
                }
            }
        }
    }
}