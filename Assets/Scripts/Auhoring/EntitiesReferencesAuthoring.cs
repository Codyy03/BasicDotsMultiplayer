using Unity.Entities;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject playerPrefabAGameObject;
    public GameObject playerPrefabBGameObject;

    public GameObject coinGameObject;
    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EntitesReferences
            {
                playerPrefabAEntitiy = GetEntity(authoring.playerPrefabAGameObject, TransformUsageFlags.Dynamic),
                playerPrefabBEntitiy = GetEntity(authoring.playerPrefabBGameObject, TransformUsageFlags.Dynamic),

                coinEntity = GetEntity(authoring.coinGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}
public struct EntitesReferences : IComponentData
{
    public Entity playerPrefabAEntitiy;
    public Entity playerPrefabBEntitiy;
    public Entity coinEntity;
}
