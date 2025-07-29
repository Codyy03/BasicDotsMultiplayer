using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct CoinPickupSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (coinTransform, coinEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<CoinTag>().WithEntityAccess())
        {
            foreach (var (playerTransfrom, playerEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player,GhostOwner>().WithEntityAccess())
            {
                float3 c = coinTransform.ValueRO.Position;
                float3 p = playerTransfrom.ValueRO.Position;

                // check if player is close enough
                if (math.distance(c, p) < 2.5)
                {
                    entityCommandBuffer.DestroyEntity(coinEntity);

                    var score = SystemAPI.GetComponent<PlayerScore>(playerEntity);
                    score.score++;

                    entityCommandBuffer.SetComponent(playerEntity,score);
                }
            }
        }
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }

}
