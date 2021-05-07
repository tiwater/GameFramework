using System;
using GameFramework.GameStructure.Util;
using Grpc.Core;

public class ChannelFactory
{

    public static Channel GetChannel()
    {
        return new Channel(GlobalConstants.RPC_SERVER, ChannelCredentials.Insecure);
    }
}
