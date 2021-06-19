using System;
using System.Threading.Tasks;
using GameFramework.Operation;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Platform.Messaging;
using GameFramework.Platform.Messaging;
using Newtonsoft.Json;
using UnityEngine;
using GameFramework.Operation.Messages;
using GameFramework.Messaging.Components.AbstractClasses;

namespace GameFramework.Operation.Components
{
    /// <summary>
    /// This class converts PlatformMessage to OperationMessage and dispatches it
    /// </summary>
    public class OperationMessageDispatcher : RunOnMessage<PlatformMessage>
    {

        public override bool RunMethod(PlatformMessage message)
        {
            HandlePlatformMessage(message);
            return true;
        }

        private void HandlePlatformMessage(PlatformMessage message)
        {

            //Convert the PlatformMessage to specific message or handle it directly
            if (message.Content.ContainsKey(PlatformMessage.TYPE))
            {
                string type = message.Content[PlatformMessage.TYPE] as string;
                if (type == PlatformMessage.TYPE_OPERATION)
                {
                    //Got an operation
                    string operation = (string)message.GetContent(PlatformMessage.CONTENT_OPERATION);
                    try
                    {
                        CharacterOperation co = CharacterOperation.Deserialize(operation);

                        OperationMessage opMessage = new OperationMessage(co);

                        GameManager.SafeQueueMessage(opMessage);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    return;
                }
            }
        }
    }
}