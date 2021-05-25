using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework.Display.Placement.Components
{

    /// <summary>
    /// Maintain a fixed distance from the specified transform
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Placement/RotateOnDrag")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class RotateOnDrag : MonoBehaviour, IDragHandler
    {
        private Vector2 deltaValue = Vector2.zero;

        public float RotateSpeed = 0.5f;

        public void OnDrag(PointerEventData eventData)
        {

            transform.Rotate(RotateSpeed * eventData.delta.y, -RotateSpeed * eventData.delta.x, 0, Space.World);
        }
    }
}