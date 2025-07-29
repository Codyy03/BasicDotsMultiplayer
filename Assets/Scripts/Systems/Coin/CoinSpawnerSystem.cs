using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct CoinSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitesReferences>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        EntitesReferences entitesReferences = SystemAPI.GetSingleton<EntitesReferences>();

        // create 10 coins
        for (int i = 0; i < 10; i++)
        {
            var coin = entityCommandBuffer.Instantiate(entitesReferences.coinEntity);

            entityCommandBuffer.SetComponent(coin, LocalTransform.FromPosition(
                new float3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10))));
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();

        // do it only once
        state.Enabled = false;
    }
}
