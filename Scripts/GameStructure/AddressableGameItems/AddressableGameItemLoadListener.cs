using System;
using GameFramework.GameStructure.Levels.Messages;
using GameFramework.Messaging.Components.AbstractClasses;

namespace GameFramework.GameStructure.Listener
{
    public class AddressableGameItemLoadListener : RunOnMessage<AddressableGameItemLoadedMessage>
    {
        /// <summary>
        /// Check if the health is zero and if so decrease the number of lives the player has.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(AddressableGameItemLoadedMessage message)
        {
            return true;
        }
    }
}