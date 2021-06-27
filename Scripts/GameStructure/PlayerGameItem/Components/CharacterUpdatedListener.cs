using System;
using System.Collections.Generic;
using GameFramework.GameStructure.Characters;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.PlayerGameItems.Messages;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.PlayerGameItems.Components
{
    /// <summary>
    /// Listen to the PlayerGameItemUpdatedMessage for a character to update the character
    /// </summary>
    [RequireComponent(typeof(CharacterHolder))]
    [AddComponentMenu("Game Framework/PlayerGameItem/Character Updated Listener")]
    public class CharacterUpdatedListener : PlayerGameItemUpdatedListener<CharacterHolder, Character>
    {
    }
}