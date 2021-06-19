using System;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.JsSupport;
using GameFramework.Messaging.Components.AbstractClasses;
using GameFramework.Operation.Messages;
using UnityEngine;

namespace GameFramework.Operation.Components
{
    /// <summary>
    /// Listen to OperationMessage and forward to the JsBehaviour
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="M"></typeparam>
    [RequireComponent(typeof(JsExtBehaviour))]
    public class OperationJsListener<T, M> : RunOnMessage<OperationMessage> where T : GameItemInstanceHolder<M> where M : GameItem
    {
        protected T itemHolder;
        protected JsExtBehaviour jsComponent;

        public override void Start()
        {
            base.Start();
            itemHolder = GetComponent<T>();
            jsComponent = GetComponent<JsExtBehaviour>();
        }

        public override bool RunMethod(OperationMessage message)
        {
            var operation = message.Operation;
            if (operation.PlayerGameItemId == itemHolder.PlayerGameItem.Id)
            {
                //It's my message
                operation.Execute(jsComponent);
            }
            return true;
        }
    }
}