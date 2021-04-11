using GameFramework.Messaging;

namespace GameFramework.Billing.Messages
{
    /// <summary>
    /// A message that is generated when a GenericGameItem is purchased.
    /// </summary>
    public class AddressableGameItemPurchasedMessage : BaseMessage
    {
        /// <summary>
        /// The number of the GenericGameItem that was purchased
        /// </summary>
        public readonly string Number;

        public AddressableGameItemPurchasedMessage(string number)
        {
            Number = number;
        }

        /// <summary>
        /// Return a representation of the message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("GenericGameItem Purchased {0}", Number);
        }
    }
}