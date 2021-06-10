using System;
using GameFramework.GameStructure.Levels.Messages;
using GameFramework.Messaging.Components.AbstractClasses;

namespace GameFramework.GameStructure.Listener
{
    public class AddressableGameItemLoadListener : RunOnMessage<AddressableGameItemLoadedMessage>
    {
        /// <summary>
        /// Called when an AddressableGameItem is loaded.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(AddressableGameItemLoadedMessage message)
        {
            return true;
        }
    }
}