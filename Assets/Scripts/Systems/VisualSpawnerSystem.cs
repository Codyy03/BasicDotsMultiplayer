using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class VisualSpawnerSystem : SystemBase
{
    private GameObject visualPrefabA;
    private GameObject visualPrefabB;

    protected override void OnCreate()
    {
        visualPrefabA = Resources.Load<GameObject>("PlayerVisualA");
        visualPrefabB = Resources.Load<GameObject>("PlayerVisualB");

        if (!visualPrefabA || !visualPrefabB)
            Debug.LogError("Missing prefabs in Resources folder.");
    }

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        var entityManager = EntityManager;

        foreach (var (charType, entity) in SystemAPI
                     .Query<PlayerCharacterType>()
                     .WithNone<VisualSpawnedTag>()
                     .WithEntityAccess())
        {
            // check if player is local
            bool isLocal = SystemAPI.HasComponent<GhostOwnerIsLocal>(entity) && SystemAPI.GetComponent<GhostOwner>(entity).NetworkId == SystemAPI.GetSingleton<NetworkId>().Value;

            var prefab = charType.Value == CharacterType.A ? visualPrefabA : visualPrefabB;
            var visualGO = Object.Instantiate(prefab);

            if (entityManager.HasComponent<LocalTransform>(entity))
            {
                var transform = entityManager.GetComponentData<LocalTransform>(entity);
                visualGO.transform.position = transform.Position;
            }

            var link = visualGO.GetComponent<EntityAnimatorLink>();
            if (link != null)
            {
                link.Animator = visualGO.GetComponent<Animator>();
                link.TrackedEntity = entity;
                link.EntityManager = entityManager;
            }

            // only local player activate camera
            if (isLocal)
            {
                var cam = visualGO.GetComponentInChildren<PlayerCameraController>();

                if (cam != null)
                {
                    cam.trackedEntity = entity;
                    cam.ActivateCamera(); 
                }
            }

            ecb.AddComponent<VisualSpawnedTag>(entity);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }

}


public struct VisualSpawnedTag : IComponentData { }
