using System;
using System.Collections.Generic;
using GameFramework.GameStructure;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.Platform.Android
{
    /// <summary>
    /// Provide bidirection Intent communication between Unity and Android
    /// </summary>
    /// This automatically hooks up the button onClick listener
    public class UnityAndroidBridge
    {
        public const string RECEIVE_ACTION = "com.tiwater.gameframework.karu.GAME_UPDATED";

        private Dictionary<string, UnityAction<Intent>> actions = new Dictionary<string, UnityAction<Intent>>();

        /// <summary>
        /// Sent Intent to Android
        /// </summary>
        /// <param name="action"></param>
        /// <param name="extMsgs"></param>
        public static void SendIntent(string action, Dictionary<string, object> extMsgs)
        {
#if UNITY_EDITOR
            //TODO: Mock in editor
#elif UNITY_ANDROID

            //AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            //Set intent
            intentObject.Call<AndroidJavaObject>("setAction", action);
            if (extMsgs != null)
            {
                foreach (var msg in extMsgs)
                {
                    intentObject.Call<AndroidJavaObject>("putExtra", msg.Key, msg.Value);
                }
            }

            GetCurrentActivity().Call("sendBroadcast", intentObject);
#endif
        }

        /// <summary>
        /// Sent Intent to Android
        /// </summary>
        /// <param name="action"></param>
        /// <param name="extMsgs"></param>
        public static void SendIntent(Intent intent)
        {

#if UNITY_EDITOR
            //TODO: Mock in editor
            GameManager.Instance.UnityAndroidBridge.OnIntent(JsonConvert.SerializeObject(intent));
#elif UNITY_ANDROID

            //AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            //Set intent
            intentObject.Call<AndroidJavaObject>("setAction", intent.Action);
            if (intent.Extras != null)
            {
                foreach (var msg in intent.Extras)
                {
                    intentObject.Call<AndroidJavaObject>("putExtra", msg.Key, msg.Value);
                }
            }

            GetCurrentActivity().Call("sendBroadcast", intentObject);
#endif
        }

        /// <summary>
        /// Add listener to Android Intent
        /// </summary>
        /// <param name="action"></param>
        /// <param name="unityAction"></param>
        /// <returns></returns>
        public UnityAction<Intent> AddIntentListener(string action, UnityAction<Intent> unityAction)
        {
            if (actions.ContainsKey(action))
            {
                //If has the action, the add new action
                actions[action] += unityAction;
            }
            else
            {
                //Otherwise store the new one
                actions[action] = unityAction;


#if UNITY_EDITOR
                //TODO: Mock in editor
#elif UNITY_ANDROID
                //And register the receiver in Android layer

                GetCurrentActivity().Call("registerIntentUnityReceiver", action);
#endif
            }
            return unityAction;
        }

        /// <summary>
        /// Remove an Intent listener
        /// </summary>
        /// <param name="unityAction"></param>
        public void RemoveIntentListener(UnityAction<Intent> unityAction)
        {
            string action = null;
            UnityAction<Intent> oprAction = null;
            foreach (var listeners in actions)
            {
                foreach(var listener in listeners.Value.GetInvocationList())
                {
                    if(listener == unityAction)
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
                        } else
                        {
                            actions[action] = oprAction;
                        }
                        return;
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

#if UNITY_EDITOR
            //TODO: Mock in editor
            return null;
#elif UNITY_ANDROID
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            return currentActivity;
#else
            return null;
#endif

        }

        /// <summary>
        /// The callback from Android layer to dispatch the Intent
        /// </summary>
        /// <param name="intent"></param>
        public void OnIntent(string intent)
        {
            //Debug.Log("Got json intent: " + intent);
            Intent intentObj = JsonConvert.DeserializeObject<Intent>(intent);
            //Debug.Log("After deserialize: " + JsonConvert.SerializeObject(intentObj));
            if (actions.ContainsKey(intentObj.Action))
            {
                actions[intentObj.Action].Invoke(intentObj);
            }
        }
    }

    public class Intent
    {
        public string Action;
        public Dictionary<string, object> Extras = new Dictionary<string, object>();

        public void PutExtra(string key, object value)
        {
            Extras[key] = value;
        }

        public object GetExtra(string key)
        {
            if (Extras.ContainsKey(key))
            {
                return Extras[key];
            }
            return null;
        }
    }
}