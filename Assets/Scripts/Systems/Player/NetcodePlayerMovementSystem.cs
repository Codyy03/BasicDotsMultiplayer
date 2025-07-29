using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRO<NetcodePlayerInput> netcopdePlayerInput, RefRW<LocalTransform> localTransfrom, RefRO<PlayerMoveSpeed> playerSpeed) in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>, RefRO<PlayerMoveSpeed>>().WithAll<Simulate>())
        {
            float3 moveVector = new float3(netcopdePlayerInput.ValueRO.inputVector.x, 0, netcopdePlayerInput.ValueRO.inputVector.y);
            localTransfrom.ValueRW.Position += moveVector * playerSpeed.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
        }
    }
}
