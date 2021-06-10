using System;
using System.Collections.Generic;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using GameFramework.Messaging;
using UnityEngine;

namespace GameFramework.GameStructure.PlayerGameItems.Messages
{
    /// <summary>
    /// A core component that holds and manages information about a scene.
    /// </summary>
    /// GameManager is where you can setup the structure of your game and holdes other key information and functionality relating to Preferences,
    /// GameStructure, Display, Localisation, Audio, Messaging and more. Please see the online help for full information.
    [AddComponentMenu("Game Framework/GameStructure/PlayerGameItem/EquipmentUpdatedMessage")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class EquipmentUpdatedMessage : BaseMessage
    {
        public readonly string CharacterId;
        /// <summary>
        /// The new PlayerGameItem
        /// </summary>
        public readonly List<PlayerGameItem> NewEquipments;

        /// <summary>
        /// The old PlayerGameItem
        /// </summary>
        public readonly List<PlayerGameItem> OldEquipments;

        public EquipmentUpdatedMessage(string characterId, List<PlayerGameItem> oldEquipments, List<PlayerGameItem> newEquipments)
        {
            CharacterId = characterId;
            OldEquipments = oldEquipments;
            NewEquipments = newEquipments;
        }
    }
}