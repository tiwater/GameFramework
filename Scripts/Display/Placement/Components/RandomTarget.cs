using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Display.Placement.Components
{
    public class RandomTarget : MonoBehaviour
    {

        /// <summary>
        /// Left up corner to limit the random target.
        /// </summary>
        [Tooltip("The min corner to limit the random target.")]
        public Vector3 MinCorner;

        /// <summary>
        /// Left up corner to limit the random target.
        /// </summary>
        [Tooltip("The max to limit the random target.")]
        public Vector3 MaxCorner;

        public Vector3 GenerateRandomTarget()
        {
            Vector3 position = new Vector3(
                Random.Range(MinCorner.x, MaxCorner.x),
                Random.Range(MinCorner.y, MaxCorner.y),
                Random.Range(MinCorner.z, MaxCorner.z)
                );
            return position;
        }
    }
}