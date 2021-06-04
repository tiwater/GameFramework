using System;
using System.Collections.Generic;
using GameFramework.GameStructure.Platform.Messaging;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.Platform.Abstract
{
    public class UnityMockBridge : UnityPlatformBridge
    {


        public override void SendPlatformMessage(PlatformMessage message)
        {
            //Forward the message as json string
            OnNativeMessage(JsonConvert.SerializeObject(message));
        }

        public override void SendPlatformMessage(string header, Dictionary<string, object> content)
        {
            //Forward the message as json string
            OnNativeMessage(JsonConvert.SerializeObject(new PlatformMessage(header, content)));
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

                            Debug.Log("Remove listener under key: " + action);
                            actions.Remove(action);
                        }
                        else
                        {
                            actions[action] = oprAction;
                        }
                        return;
                    }
                }
            }
        }
    }
}