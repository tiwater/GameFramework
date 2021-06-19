using System;
using GameFramework.Operation;
using GameFramework.Messaging;

namespace GameFramework.Operation.Messages
{
    /// <summary>
    /// The message to encapsulate character operation
    /// </summary>
    public class OperationMessage : BaseMessage
    {
        public readonly CharacterOperation Operation;
        public OperationMessage(CharacterOperation operation)
        {
            this.Operation = operation;
        }
    }
}