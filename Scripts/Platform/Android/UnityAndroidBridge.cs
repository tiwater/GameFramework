using System;
using System.Collections.Generic;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Platform.Messaging;
using GameFramework.Platform.Abstract;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.Platform.Android
{
    /// <summary>
    /// Provide bidirection Intent communication between Unity and Android
    /// </summary>
    /// This automatically hooks up the button onClick listener
    public class UnityAndroidBridge : UnityPlatformBridge
    {

        public override void SendPlatformMessage(PlatformMessage message)
        {


#if UNITY_EDITOR
            //TODO: Mock in editor
            GameManager.Instance.UnityPlatformBridge.OnNativeMessage(JsonConvert.SerializeObject(message));
#elif UNITY_ANDROID

            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            //Set intent
            intentObject.Call<AndroidJavaObject>("setAction", message.Header);
            if (message.Content != null)
            {
                foreach (var msg in message.Content)
                {
                    intentObject.Call<AndroidJavaObject>("putExtra", msg.Key, msg.Value);
                }
            }

            GetCurrentActivity().Call("sendBroadcast", intentObject);
#endif
        }

        public override void SendPlatformMessage(string header, Dictionary<string, object> content)
        {
#if UNITY_EDITOR
            //TODO: Mock in editor
#elif UNITY_ANDROID

            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            //Set intent
            intentObject.Call<AndroidJavaObject>("setAction", header);
            if (content != null)
            {
                foreach (var msg in content)
                {
                    intentObject.Call<AndroidJavaObject>("putExtra", msg.Key, msg.Value);
                }
            }

            GetCurrentActivity().Call("sendBroadcast", intentObject);
#endif
        }

        public override UnityAction<PlatformMessage> AddMessageListener(string header, UnityAction<PlatformMessage> unityAction)
        {

            if (actions.ContainsKey(header))
            {
                //If has the action, the add new action
                actions[header] += unityAction;
            }
            else
            {
                //Otherwise store the new one
                actions[header] = unityAction;


#if UNITY_EDITOR
                //TODO: Mock in editor
#elif UNITY_ANDROID
                //And register the receiver in Android layer

                GetCurrentActivity().Call("registerIntentUnityReceiver", header);
#endif
            }
            return unityAction;
        }

        public override void RemoveMessageListener(UnityAction<PlatformMessage> unityAction)
        {

            string action = null;
            UnityAction<PlatformMessage> oprAction = null;
            foreach (var listeners in actions)
            {
                foreach (var listener in listeners.Value.GetInvocationList())
                {
                    if (listener == unityAction)
                    {
                        Debug.Log("Found the listener to remove");
                        action = listeners.Key;
                        oprAction = listeners.Value;
                        oprAction -= unityAction;
                        if (oprAction == null)
                        {
                            //The listeners under the action are all removed, unregister the receiver in Android layer too

#if UNITY_EDITOR
                            //TODO: Mock in editor
#elif UNITY_ANDROID
                            GetCurrentActivity().Call("unRegisterIntentUnityReceiver", action);
#endif
                            Debug.Log("Remove listener under key: " + action);
                            actions.Remove(action);
                        }
                        else
                        {
                            actions[action] = oprAction;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get current Activity reference
        /// </summary>
        /// <returns></returns>
        public static AndroidJavaObject GetCurrentActivity()
        {

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            return currentActivity;

        }


        public override void StartService(string packageName, string className, string action = null, Dictionary<string, object> content = null)
        {

#if UNITY_EDITOR
            //None in editor
#elif UNITY_ANDROID


            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            //Set intent
            intentObject.Call<AndroidJavaObject>("setClassName", packageName, className);
            if (!string.IsNullOrEmpty(action))
            {
                intentObject.Call<AndroidJavaObject>("setAction", action);
            }
            if (content != null)
            {
                foreach (var msg in content)
                {
                    intentObject.Call<AndroidJavaObject>("putExtra", msg.Key, msg.Value);
                }
            }

            GetCurrentActivity().Call<AndroidJavaObject>("startService", intentObject);
#endif
        }

        public override void StopService(string packageName, string className)
        {
#if UNITY_EDITOR
            //None in editor
#elif UNITY_ANDROID


            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            //Set intent
            intentObject.Call<AndroidJavaObject>("setClassName", packageName, className);

            GetCurrentActivity().Call<AndroidJavaObject>("stopService", intentObject);
#endif
        }
    }
}