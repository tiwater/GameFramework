using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.Messaging;

namespace GameFramework.GameStructure.Levels.Messages
{
    /// <summary>
    /// A message that is generated when the ExtGameItem Meta is loaded.
    /// </summary>
    public class GameItemMetaLoadedMessage : BaseMessage
    {
        public GameItemMetaLoadedMessage()
        {
        }

        /// <summary>
        /// Return a representation of the message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GameItemMeta is loaded";
        }
    }
}