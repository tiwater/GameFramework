using System;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

namespace GameFramework.GameStructure.Characters.ObjectModel
{
    /// <summary>
    /// Mark the equipment slot for the GameObject
    /// </summary>
    /// Mark the equipment slot for the GameObject, so the equipable assets can be mounted properly.
    [AddComponentMenu("Game Framework/GameStructure/Characters/Equipment Slot")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class EquipmentSlot : MonoBehaviour
    {
        public Slot Slot;
    }
}