using System;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Characters;
using GameFramework.GameStructure.PlayerGameItems.Messages;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.PlayerGameItems.Components
{
    /// <summary>
    /// Listen to the EquipmentUpdatedMessage to update the equipments for a character
    /// </summary>
    [RequireComponent(typeof(CharacterHolder))]
    public class EquipmentUpdatedListener : RunOnMessage<EquipmentUpdatedMessage>
    {

        protected CharacterHolder itemHolder;
        public override void Start()
        {
            base.Start();
            itemHolder = GetComponent<CharacterHolder>();
        }

        public override bool RunMethod(EquipmentUpdatedMessage message)
        {
            if (itemHolder != null && message.CharacterId == itemHolder.PlayerGameItem.Id)
            {
                //It's me to update the equipment
                PlayerGameItem character = GameManager.Instance.PlayerGameItems.GetPlayerGameItemById(message.CharacterId);
                //Update the equipment list
                character.Equipments = message.NewEquipments;
                //Rebind
                ((CharacterHolder)this.itemHolder).BindCharacterPGI(character, character.PrefabType);
            }

            return true;
        }
    }
}