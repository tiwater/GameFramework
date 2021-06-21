using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Display.Placement.Components
{
    /// <summary>
    /// By setting the main camera as the canvas' render camera, it can make the canvas'
    /// content as the 2D background of the 3D world
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class ConfigBackgroundCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}