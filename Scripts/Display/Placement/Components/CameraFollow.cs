using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure;
using UnityEngine;


namespace GameFramework.Display.Placement.Components
{
    /// <summary>
    /// Make the camera to follow the target. And can support rotate camera around target.
    /// The camera will always look at the target
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        private bool initialized = false;

        /// <summary>
        /// The target this camera to follow
        /// </summary>
        public Transform Target;
        public float Smoothing = 5f;

        public bool SupportRotate = true;
        public float RotateSpeed = 250f;

        /// <summary>
        /// The time threshold to start drag the camera to rotate around the target
        /// </summary>
        public float DragThreshold = 0.5f;

        private bool isDown = false;
        private float dragTime = 0;

        private Vector3 offset;

        private float touchFactor = 0.05f;

        private void LateUpdate()
        {
            if (!initialized)
            {
                GameObject sceneManager = GameObject.Find("SceneManager");
                if (sceneManager != null && sceneManager.GetComponent<SceneItemInstanceManager>() != null
                && sceneManager.GetComponent<SceneItemInstanceManager>().PlayerCharacterHolder != null)
                {
                    var renderer = sceneManager.GetComponent<SceneItemInstanceManager>()
                        .PlayerCharacterHolder.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        //Get the character
                        Target = sceneManager.GetComponent<SceneItemInstanceManager>()
                            .PlayerCharacterHolder.transform;
                        //Record the offset
                        //Because the models stand on origin, so the center has an offset to the origin
                        var centerOffset = new Vector3(0, renderer.bounds.center.y, 0);
                        transform.position = transform.position + centerOffset;
                        offset = transform.position - Target.position;
                        initialized = true;
                    }
                }
            }
            if (initialized)
            {
                Vector3 targetCamPos = Target.position + offset;
                //transform.position = Vector3.Lerp(transform.position, targetCamPos, Smoothing * Time.deltaTime);
                transform.position = targetCamPos;
                //Handle rotate
                if (SupportRotate)
                {
                    //Already press down or not?
                    if (!isDown)
                    {
                        if (1 == UnityEngine.Input.touchCount || UnityEngine.Input.GetMouseButton(0))
                        {
                            //Just press down, start record the press time
                            isDown = true;
                            dragTime = 0;
                        }
                    }
                    else
                    {
                        if (1 == UnityEngine.Input.touchCount || UnityEngine.Input.GetMouseButton(0))
                        {
                            dragTime += Time.deltaTime;
                            if (dragTime > DragThreshold)
                            {
                                float dx = 0;
                                float dy = 0;
                                if (1 == UnityEngine.Input.touchCount)
                                {
                                    //Press down long enough to start drag
                                    Touch touch = UnityEngine.Input.GetTouch(0);
                                    Vector2 deltaPos = touch.deltaPosition;
                                    dx = deltaPos.x * touchFactor;
                                    dy = deltaPos.y * touchFactor;
                                }
                                else
                                {
                                    dx = UnityEngine.Input.GetAxis("Mouse X");
                                    dy = UnityEngine.Input.GetAxis("Mouse Y");
                                }
                                //Rotate around z axis
                                transform.RotateAround(Target.position, Vector3.up, RotateSpeed * dx * Time.deltaTime);
                                //Calculate the new forward and right
                                Vector3 forward = transform.position - Target.position;
                                Vector3 right = Vector3.Cross(Vector3.up, forward);
                                //Calculate the angle between up axis
                                var angle = Vector3.Angle(forward, Vector3.up);
                                //The angle to rotate
                                float ry = RotateSpeed * dy * Time.deltaTime;
                                angle += ry;
                                //Limit the rotate range
                                if (angle <= 0)
                                {
                                    ry -= (angle - 0.1f);
                                }
                                else if (angle >= 180)
                                {
                                    ry -= (angle - 180 + 0.1f);
                                }
                                //Rotate around right axis
                                transform.RotateAround(Target.position, right, ry);
                                //Update the new offset
                                offset = transform.position - Target.position;
                            }
                        }
                        else
                        {
                            //Not press down 1 point
                            isDown = false;
                        }
                    }
                }
            }

        }
    }
}