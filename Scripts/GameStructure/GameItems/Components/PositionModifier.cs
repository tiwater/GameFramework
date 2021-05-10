using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;

namespace GameFramework.GameStructure.GameItems.Components
{
    /// <summary>
    /// Provide offset, direction correction for the model
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Common/Position Modifier")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class PositionModifier : MonoBehaviour
    {
        [Tooltip("The offset of the game object.")]
        public Vector3 Offset;

        [Tooltip("The axis of the forward direction of game object.")]
        public AxisDirection ForwardDirection;
    }
}