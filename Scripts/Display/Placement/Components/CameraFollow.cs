using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool initialized = false;

    public Transform Target;
    public float Smoothing = 5f;

    Vector3 offset;

    private void FixedUpdate()
    {
        if (!initialized)
        {
            GameObject sceneManager = GameObject.Find("SceneManager");
            if (sceneManager != null && sceneManager.GetComponent<SceneItemInstanceManager>() != null
            && sceneManager.GetComponent<SceneItemInstanceManager>().PlayerCharacterHolder != null)
            {
                Target = sceneManager.GetComponent<SceneItemInstanceManager>()
                    .PlayerCharacterHolder.transform.GetChild(0).transform;
                offset = transform.position - Target.position;
                initialized = true;
            }
        }
        if (initialized)
        {
            Vector3 targetCamPos = Target.position + offset;
            //transform.position = Vector3.Lerp(transform.position, targetCamPos, Smoothing * Time.deltaTime);
            transform.position = targetCamPos;
        }

    }
}
