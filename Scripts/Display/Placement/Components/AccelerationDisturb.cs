using GameFramework.GameStructure.JsSupport;
using GameFramework.Operation;
using UnityEngine;


namespace GameFramework.Display.Placement.Components
{
    /// <summary>
    /// Use the big/sudden acceleration input to give the object a vibrate effect
    /// </summary>
    public class AccelerationDisturb : MemsDriver
    {
        //private int index = 0;

        /// <summary>
        /// Support the change on Y axis.
        /// </summary>
        [Tooltip("Support the change on Y axis.")]
        public bool SupportY = false;
        public float PushForce = 1;
        public float VibrativeFreq = 8;
        public float ShakeVolve = 0.00002f;
        public float ShakeDuration = 4;

        public CameraFollow CameraFollowComp;
        public MoveToPosition MoveToPositionComp;

        private float t = 0;
        private Vector3 vibrativeForce = Vector3.zero;
        private Vector3 lastVibrativeForce = Vector3.zero;

        private Vector3 originalPosition = Vector3.zero;
        private bool disturbing = false;
        private JsExtBehaviour jsComp;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            if (CameraFollowComp == null)
            {
                //Try to get the canvas for background by ConfigBackgroundCamera
                var cameraComps = Resources.FindObjectsOfTypeAll(typeof(CameraFollow));
                foreach (var comp in cameraComps)
                {
                    //Suppose we only have one
                    CameraFollowComp = (CameraFollow)comp;
                    break;
                }
            }
            if (MoveToPositionComp == null)
            {
                MoveToPositionComp = GetComponent<MoveToPosition>();
            }
            jsComp = GetComponent<JsExtBehaviour>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            AccelerationDriveMove();

        }

        Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
        {
            Vector3 newValue = Vector3.Lerp(prevValue, UnityEngine.Input.acceleration, lowPassFilterFactor);
            return newValue;
        }

        private void AccelerationDriveMove()
        {
            var accForce = lowPassValue;
            lowPassValue = LowPassFilterAccelerometer(lowPassValue);

            var delta = lowPassValue - accForce;
            AccelerationDeltaDrive(delta);
        }

        private void AccelerationDeltaDrive(Vector3 accDelta)
        {
            if (!SupportY)
            {
                accDelta.z = accDelta.y;
                accDelta.y = 0;
            }
            //if ((index++) % 15 == 0)
            //{
            //    Debug.Log("delta:" + accDelta.sqrMagnitude);
            //}
            if (disturbing)
            {
                t = t + Time.deltaTime;
            }
            if (accDelta.sqrMagnitude > ShakeVolve)
            {
                vibrativeForce = lastVibrativeForce + accDelta * 100;
                if (vibrativeForce.sqrMagnitude > 1)
                {
                    vibrativeForce.Normalize();
                }
                //Show dizzy animation
                if (jsComp != null)
                {
                    //TODO: The animation should repeat until the vibration ends
                    jsComp.PerformOperation(new NormalShowAction("dummy", GameItemHolder.PlayerGameItem.Id, "Dizzy"));
                }
                if (!disturbing)
                {
                    originalPosition = transform.position;
                    disturbing = true;
                }
                t = 0;
            }
            if (disturbing)
            {
                lastVibrativeForce = vibrativeForce * Mathf.Exp(-t / 2) * Mathf.Sin(t * VibrativeFreq);

                if (t < ShakeDuration || lastVibrativeForce.sqrMagnitude > 0.0025f)
                {
                    //RigidbodyComp.AddForce(lastVibrativeForce);
                    CameraFollowComp.enabled = false;
                    MoveToPositionComp.enabled = false;

                    //Rotate the move direction according to camera
                    Vector3 movement;
                    if (SupportY)
                    {
                        movement = Camera.main.transform.rotation * lastVibrativeForce;
                    }
                    else
                    {
                        Vector3 cameraRoate = Camera.main.transform.rotation.eulerAngles;
                        movement = (Quaternion.AngleAxis(cameraRoate.y, Vector3.up) * lastVibrativeForce);
                    }

                    RigidbodyComp.MovePosition(originalPosition + movement * PushForce);
                }
                else
                {
                    //TODO: Support customized action when the vibration ends
                    RigidbodyComp.MovePosition(originalPosition);
                    CameraFollowComp.enabled = true;
                    MoveToPositionComp.enabled = true;
                    disturbing = false;
                }
            }
        }
    }
}