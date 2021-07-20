using UnityEngine;

namespace GameFramework.Display.Placement.Components
{
    /// <summary>
    /// This component can drive the object to move by accelerometer
    /// </summary>
    public class AccerlerationDriver : MemsDriver
    {
        public float Speed = 20;
        public float Valve = 0.00001f;

        private int index = 0;



        // Update is called once per frame
        void FixedUpdate()
        {
            AccelerationDriveMove();

        }

        /// <summary>
        /// Drive the object to move by accelerometer
        /// </summary>
        private void AccelerationDriveMove()
        {
            lowPassValue = LowPassFilterAccelerometer(lowPassValue);
            if ((index++) % 15 == 0)
            {
                Debug.Log(string.Format("lowPassValue: {0}, {1}", lowPassValue.magnitude, lowPassValue.sqrMagnitude));
            }
            if (lowPassValue.magnitude > Valve)
            {

                var accForce = lowPassValue;

                // remap the device acceleration axis to game coordinates:
                // Device's XY plane to game's XZ plane
                accForce.x = UnityEngine.Input.acceleration.x;
                accForce.z = UnityEngine.Input.acceleration.y;
                accForce.y = 0;

                // clamp acceleration vector to the unit sphere 
                if (accForce.sqrMagnitude > 1)
                {
                    accForce.Normalize();
                }

                accForce *= Speed * Time.deltaTime;

                if ((index) % 15 == 0)
                {
                    Debug.Log(string.Format("mems: {0}, {1}, {2}", UnityEngine.Input.acceleration.x,
                        UnityEngine.Input.acceleration.y, UnityEngine.Input.acceleration.z));
                }
                //Rotate the move direction according to camera
                Vector3 cameraRoate = Camera.main.transform.rotation.eulerAngles;
                accForce = (Quaternion.AngleAxis(cameraRoate.y, Vector3.up) * accForce);

                //Move object
                //TODO: support rotate the object to look at the move direction
                if (RigidbodyComp != null)
                {
                    RigidbodyComp.MovePosition(transform.position + accForce);
                }
                else
                {
                    transform.Translate(accForce);
                }
            }
        }

        /// <summary>
        /// Low pass filter for the acceleration values
        /// </summary>
        /// <param name="prevValue"></param>
        /// <returns></returns>
        Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
        {
            Vector3 newValue = Vector3.Lerp(prevValue, UnityEngine.Input.acceleration, lowPassFilterFactor);
            return newValue;
        }
    }
}