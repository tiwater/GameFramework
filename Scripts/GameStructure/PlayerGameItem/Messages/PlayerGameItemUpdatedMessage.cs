using System;
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
    [AddComponentMenu("Game Framework/GameStructure/PlayerGameItem/PlayerGameItemChangedMessage")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class PlayerGameItemUpdatedMessage : BaseMessage
    {
        /// <summary>
        /// The new PlayerGameItem
        /// </summary>
        public readonly PlayerGameItem NewItem;

        /// <summary>
        /// The old PlayerGameItem
        /// </summary>
        public readonly PlayerGameItem OldItem;

        public PlayerGameItemUpdatedMessage(PlayerGameItem oldItem, PlayerGameItem newItem)
        {
            OldItem = oldItem;
            NewItem = newItem;
        }
    }
}