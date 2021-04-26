using GameFramework.Messaging;

namespace GameFramework.GameStructure.Levels.Messages
{
    /// <summary>
    /// A message that is generated when the ExtGameItem Meta is loaded.
    /// </summary>
    public class AddressableGameItemLoadedMessage : BaseMessage
    {
        public AddressableGameItemLoadedMessage()
        {
        }

        /// <summary>
        /// Return a representation of the message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AddressableGameItem is loaded";
        }
    }
}
