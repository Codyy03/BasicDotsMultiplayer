using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

public class NetCodePlayerInputAuthoring : MonoBehaviour
{
    public class Baker : Baker<NetCodePlayerInputAuthoring>
    {
        public override void Bake(NetCodePlayerInputAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NetcodePlayerInput());
        }
    }
}
public struct NetcodePlayerInput : IInputComponentData
{
    public float2 inputVector;
}
