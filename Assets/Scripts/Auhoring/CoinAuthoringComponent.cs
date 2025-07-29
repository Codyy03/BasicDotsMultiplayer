using Unity.Entities;
using UnityEngine;

public class CoinAuthoringComponent : MonoBehaviour
{
    public class Baker : Baker<CoinAuthoringComponent>
    {
        public override void Bake(CoinAuthoringComponent authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CoinTag());
        }
    }
}
public struct CoinTag : IComponentData { }
