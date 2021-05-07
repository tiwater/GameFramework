using System;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;
using Newtonsoft.Json;
using Tiwater;
using UnityEngine;

public class RpcTest
{
    public async Task TestRpc()
    {
        try
        {
            Channel channel = ChannelFactory.GetChannel();

            var client = new Greeter.GreeterClient(channel);
            string user = "My Friends";

            var reply = await client.SayHelloAsync(new HelloRequest { Name = user });

            TestRpc1();

            channel.ShutdownAsync().Wait();
            Debug.Log(reply.Message);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }


    public void TestRpc1()
    {
        try
        {
            Channel channel = ChannelFactory.GetChannel();

            var client = new CreationService.CreationServiceClient(channel);

            var reply = client.GetCreationsList(new Google.Protobuf.WellKnownTypes.Empty());

            channel.ShutdownAsync().Wait();
            Debug.Log(reply.List);
            Debug.Log(JsonConvert.SerializeObject(reply));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
