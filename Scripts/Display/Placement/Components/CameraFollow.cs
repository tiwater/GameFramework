using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool initialized = false;

    public Transform Target;
    public float Smoothing = 5f;

    public bool SupportRotate = true;
    public float RotateSpeed = 50f;

    public float DragThreshold = 0.5f;

    private bool isDown = false;
    private float dragTime = 0;

    private Vector3 offset;

    private Touch oldTouch1;
    private Touch oldTouch2;

    private float touchFactor = 0.05f;

    private void FixedUpdate()
    {
        if (!initialized)
        {
            GameObject sceneManager = GameObject.Find("SceneManager");
            if (sceneManager != null && sceneManager.GetComponent<SceneItemInstanceManager>() != null
            && sceneManager.GetComponent<SceneItemInstanceManager>().PlayerCharacterHolder != null)
            {
                //Get the character
                Target = sceneManager.GetComponent<SceneItemInstanceManager>()
                    .PlayerCharacterHolder.transform;
                //Record the offset
                offset = transform.position - Target.position;
                initialized = true;
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
                    if (1 == Input.touchCount || Input.GetMouseButton(0))
                    {
                        //Just press down, start record the press time
                        isDown = true;
                        dragTime = 0;
                    }
                } else
                {
                    if (1 == Input.touchCount || Input.GetMouseButton(0))
                    {
                        dragTime += Time.deltaTime;
                        if (dragTime > DragThreshold)
                        {
                            float dx = 0;
                            float dy = 0;
                            if (1 == Input.touchCount)
                            {
                                //Press down long enough to start drag
                                Touch touch = Input.GetTouch(0);
                                Vector2 deltaPos = touch.deltaPosition;
                                dx = deltaPos.x * touchFactor;
                                dy = deltaPos.y * touchFactor;
                            } else
                            {
                                dx = Input.GetAxis("Mouse X");
                                dy = Input.GetAxis("Mouse Y");
                            }
                            //Rotate around z axis
                            transform.RotateAround(Target.position, Vector3.up, RotateSpeed * dx);
                            //Calculate the new forward and right
                            Vector3 forward = transform.position - Target.position;
                            Vector3 right = Vector3.Cross(Vector3.up, forward);
                            //Calculate the angle between up axis
                            var angle = Vector3.Angle(forward, Vector3.up);
                            //The angle to rotate
                            float ry = RotateSpeed * dy;
                            angle += ry;
                            //Limit the rotate range
                            if (angle <= 0)
                            {
                                ry -= (angle - 0.1f);
                            } else if(angle >= 180)
                            {
                                ry -= (angle - 180 + 0.1f);
                            }
                            //Rotate around right axis
                            transform.RotateAround(Target.position, right, ry);
                            //Update the new offset
                            offset = transform.position - Target.position;
                        }
                    } else
                    {
                        //Not press down 1 point
                        isDown = false;
                    }
                }
            }
        }

    }
}
