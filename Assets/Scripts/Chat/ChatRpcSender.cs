using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

public static class ChatRpcSender
{
    // send message 
    public static void SendChat(EntityCommandBuffer ecb, Entity connEntity, FixedString128Bytes message)
    {
        var rpcEntity = ecb.CreateEntity();
        ecb.AddComponent(rpcEntity, new OutgoingChatMessage { Message = message });
        ecb.AddComponent(rpcEntity, new SendRpcCommandRequest { TargetConnection = connEntity });
    }
}
