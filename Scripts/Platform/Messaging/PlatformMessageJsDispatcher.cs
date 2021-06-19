using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure;
using GameFramework.GameStructure.JsSupport;
using GameFramework.GameStructure.Platform.Messaging;
using GameFramework.Messaging.Components.AbstractClasses;
using GameFramework.Operation.Messages;
using UnityEngine;

namespace GameFramework.Platform.Messaging
{
    /// <summary>
    /// The PlatformMessage dispatcher for JsExtBehaviour, it will dispatch the PlatformMessage to js env
    /// </summary>
    [RequireComponent(typeof(JsExtBehaviour))]
    [AddComponentMenu("Game Framework/Platform/PlatformMessageJsDispatcher")]
    public class PlatformMessageJsDispatcher : RunOnMessage<PlatformMessage>
    {
        protected JsExtBehaviour jsComponent;

        public override void Start()
        {
            base.Start();
            jsComponent = GetComponent<JsExtBehaviour>();
        }

        public override bool RunMethod(PlatformMessage message)
        {
            //Foward the PlatformMessage to js environment
            //TODO: Do filter before send to js
            jsComponent.OnMessage(message);
            return true;
        }
    }
}