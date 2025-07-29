using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public CharacterType characterType;
    public float moveSpeed;
    public int playerScore;
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Player());
            AddComponent(entity, new PlayerMoveSpeed
            {
                moveSpeed = authoring.moveSpeed,
            });

            AddComponent(entity, new PlayerCharacterType 
            { 
                Value = authoring.characterType 
            });

            AddComponent(entity, new PlayerScore
            {
                score = authoring.playerScore,
            });
        }
    }
}
public struct Player : IComponentData { }

public struct PlayerMoveSpeed : IComponentData
{
    public float moveSpeed;
}
[GhostComponent]
public struct PlayerCharacterType : IComponentData
{
    [GhostField]
    public CharacterType Value;
}
public struct PlayerScore : IComponentData
{
    public int score;
}

