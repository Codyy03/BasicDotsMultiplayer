using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct GoInGameServerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitesReferences>();
        state.RequireForUpdate<NetworkId>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        var refs = SystemAPI.GetSingleton<EntitesReferences>();

        foreach (var (req, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestRpc>().WithEntityAccess())
        {
            var conn = req.ValueRO.SourceConnection;
            var netId = SystemAPI.GetComponent<NetworkId>(conn).Value;

            ecb.AddComponent<NetworkStreamInGame>(conn);

            // Assign prefab A or B depending on NetworkId
            var prefab = netId == 1 ? refs.playerPrefabAEntitiy : refs.playerPrefabBEntitiy;

            var charType = netId == 1 ? CharacterType.A : CharacterType.B;

            Entity playerEntity = ecb.Instantiate(prefab);

            ecb.SetComponent(playerEntity, LocalTransform.FromPosition(new Unity.Mathematics.float3(UnityEngine.Random.Range(-10, 10), 0, 0)));

            ecb.AddComponent(playerEntity, new GhostOwner { NetworkId = netId });

            // sendig message
            ChatRpcSender.SendChat(ecb, conn, $"Witaj Graczu #{netId}");

            ecb.AppendToBuffer(conn, new LinkedEntityGroup { Value = playerEntity });
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
public enum CharacterType : byte { A, B }

