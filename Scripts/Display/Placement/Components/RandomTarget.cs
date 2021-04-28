using System;
using Puerts;
using PuertsExtension;
using UnityEngine;

namespace GameFramework.Display.Placement.Components
{
    public class RandomTarget : JsBehaviour//MonoBehaviour
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

        public GenericDelegate NextTarget;


        //public Vector3 GenerateRandomTarget()
        //{
        //    Vector3 position = new Vector3(
        //        UnityEngine.Random.Range(MinCorner.x, MaxCorner.x),
        //        UnityEngine.Random.Range(MinCorner.y, MaxCorner.y),
        //        UnityEngine.Random.Range(MinCorner.z, MaxCorner.z)
        //        );
        //    return position;
        //}


        /// <summary>
        /// Demo the JS version
        /// </summary>
        /// <returns></returns>
        public Vector3 GenerateRandomTarget()
        {
            Vector3 position;
            if (NextTarget != null)
            {
                position = NextTarget.Func<Vector3, Vector3, Vector3>(MaxCorner, MinCorner);
            }
            else
            {
                position = Vector3.zero;
            }
            return position;
        }
    }
}