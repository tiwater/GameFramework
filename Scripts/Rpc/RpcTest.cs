using System;
using System.Threading.Tasks;
using Creation;
using Grpc.Core;
using Helloworld;
using Newtonsoft.Json;
using UnityEngine;
using Userinfo;

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
            await TestRpc1();
            TestRpc2();

            channel.ShutdownAsync().Wait();
            Debug.Log(reply.Message);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }


    public async Task TestRpc1()
    {
        try
        {
            Channel channel = ChannelFactory.GetChannel();

            var client = new CreationProvider.CreationProviderClient(channel);

            //var reply = await client.GetCreationAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var request = new GetCreationRequest();
            request.World = "lido";
            var reply = await client.GetCreationAsync(request);

            channel.ShutdownAsync().Wait();
            Debug.Log(JsonConvert.SerializeObject(reply));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void TestRpc2()
    {
        try
        {
            Channel channel = ChannelFactory.GetChannel();

            var client = new UserInfo.UserInfoClient(channel);

            var reply = client.GetUserInfo(new Google.Protobuf.WellKnownTypes.Empty());

            channel.ShutdownAsync().Wait();
            Debug.Log(JsonConvert.SerializeObject(reply));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
