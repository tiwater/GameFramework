using System;
using GameFramework.Messaging;

public class PlayerGameItemLoadedMessage : BaseMessage
{

    public PlayerGameItemLoadedMessage()
    {
    }

    /// <summary>
    /// Return a representation of the message
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "PlayerGameItem is loaded";
    }

}
