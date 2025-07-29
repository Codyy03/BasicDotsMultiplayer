using Unity.Entities;
using UnityEngine;

public class PlayerAnimatorAuthoring : MonoBehaviour
{
    public Animator animator;
    public class Baker : Baker<PlayerAnimatorAuthoring>
    {
        public override void Bake(PlayerAnimatorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, authoring.animator);
        }
    }
}

