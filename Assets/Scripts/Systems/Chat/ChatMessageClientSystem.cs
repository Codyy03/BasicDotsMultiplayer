using Unity.Burst;
using Unity.Entities;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct ChatMessageClientSystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ( var (msg, entity) in SystemAPI.Query<RefRO<OutgoingChatMessage>>().WithEntityAccess())
        {
            var mesage = msg.ValueRO.Message;

            UnityEngine.Debug.Log($"Client recived chat {mesage}");

            entityCommandBuffer.DestroyEntity(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }

}
