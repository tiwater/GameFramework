using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Platform.Messaging;
using GameFramework.Messaging;
using UnityEngine;

namespace GameFramework.Platform.Messaging
{
    /// <summary>
    /// The PlatformMessage dispatcher, it will dispatch the PlatformMessage from native
    /// platform to the message queue
    /// </summary>
    [AddComponentMenu("Game Framework/Platform/PlatformMessageDispatcher")]
    public class PlatformMessageDispatcher : GmStartDependBehaviour
    {
        protected List<string> ListenedMessageHeaders = new List<string>();

        /// <summary>
        /// Register the message headers this dispatcher listens.
        /// </summary>
        protected virtual void RegisterListenedMessageHeaders()
        {
            ListenedMessageHeaders.Add(PlatformMessage.DEFAULT_MESSAGE_HEADER);
        }

        protected override void GmReadyStart()
        {
            //Register the message headers
            RegisterListenedMessageHeaders();
            //Register listener for each header, then all messages from native platform will be sent here
            foreach (var header in ListenedMessageHeaders)
            {
                //The UnityPlatformBridge in GameManager will handle the real native message receiving
                GameManager.Instance.UnityPlatformBridge.AddMessageListener(header, OnMessage);
            }
        }

        private void OnDestroy()
        {

            foreach (var header in ListenedMessageHeaders)
            {
                GameManager.Instance.UnityPlatformBridge.RemoveMessageListener(OnMessage);
            }
        }

        /// <summary>
        /// The listener for the native message, it will dispatch the PlatformMessage to the message queue
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual void OnMessage(PlatformMessage message)
        {
            GameManager.SafeQueueMessage(message);
        }
    }
}