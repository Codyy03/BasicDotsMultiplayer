using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct ChatMessageReceiveSystem : ISystem
{
 //   [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (rpc, rpcEntity) in SystemAPI.Query<RefRO<ChatMessageRpc>>().WithEntityAccess())
        {
            var message = rpc.ValueRO.Message;

            foreach (var (conn, connEntity) in SystemAPI.Query<RefRO<NetworkStreamConnection>>().WithEntityAccess())
            {
                var msg = new OutgoingChatMessage { Message = message };

                // check if the entity already has a message component - update or add
                if (SystemAPI.HasComponent<OutgoingChatMessage>(connEntity))
                    entityCommandBuffer.SetComponent(connEntity, msg);
                else
                    entityCommandBuffer.AddComponent(connEntity, msg);
            }

            entityCommandBuffer.DestroyEntity(rpcEntity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
public struct OutgoingChatMessage : IRpcCommand
{
    public FixedString128Bytes Message;
}
