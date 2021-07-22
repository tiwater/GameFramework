using System;
using System.Collections.Generic;
using GameFramework.GameStructure.Platform.Messaging;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace GameFramework.Platform.Abstract
{
    /// <summary>
    /// The communication bridge between Unity and the native platform
    /// </summary>
    public abstract class UnityPlatformBridge
    {
        protected Dictionary<string, UnityAction<PlatformMessage>> actions = new Dictionary<string, UnityAction<PlatformMessage>>();

        /// <summary>
        /// Send message to native platform
        /// </summary>
        /// <param name="message"></param>
        public abstract void SendPlatformMessage(PlatformMessage message);

        /// <summary>
        /// Send message to native platform
        /// </summary>
        /// <param name="header">Message header</param>
        /// <param name="content">Message content</param>
        public abstract void SendPlatformMessage(string header, Dictionary<string, object> content);

        /// <summary>
        /// Add listener for specified message header
        /// </summary>
        /// <param name="header">Message header to listener</param>
        /// <param name="unityAction"></param>
        /// <returns></returns>
        public abstract UnityAction<PlatformMessage> AddMessageListener(string header, UnityAction<PlatformMessage> unityAction);

        /// <summary>
        /// Remove a message listener
        /// </summary>
        /// <param name="unityAction">The message listener</param>
        public abstract void RemoveMessageListener(UnityAction<PlatformMessage> unityAction);

        /// <summary>
        /// Start a system service
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="className"></param>
        public abstract void StartService(string packageName, string className);

        /// <summary>
        /// Stop the specified service
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="className"></param>
        public abstract void StopService(string packageName, string className);

        /// <summary>
        /// The callback from native layer when a native message is received
        /// </summary>
        /// <param name="jsonMessage">The json format of the PlatformMessage converted from the native message by the UnityPlatformBridge</param>
        public void OnNativeMessage(string jsonMessage)
        {
            //Deserialize
            PlatformMessage message = JsonConvert.DeserializeObject<PlatformMessage>(jsonMessage);
            //Dispatch the message
            if (actions.ContainsKey(message.Header))
            {
                actions[message.Header].Invoke(message);
            }
        }
    }

}