using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class EntityAnimatorLink : MonoBehaviour
{
    public Animator Animator { get; set; }
    public Entity TrackedEntity { get; set; }
    public EntityManager EntityManager { get; set; }

    private AnimationsController animController;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        animController = GetComponent<AnimationsController>();
    }

    private void Update()
    {
        if (!EntityManager.Exists(TrackedEntity))
            return;

        UpdateTransformFromEntity();
        UpdateAnimationFromInput();
    }

    private void UpdateTransformFromEntity()
    {
        if (EntityManager.HasComponent<LocalToWorld>(TrackedEntity))
        {
            var transformData = EntityManager.GetComponentData<LocalToWorld>(TrackedEntity);
            transform.position = transformData.Position;
            transform.rotation = transformData.Rotation;
        }
    }
    private void UpdateAnimationFromInput()
    {
        if (!EntityManager.HasComponent<NetcodePlayerInput>(TrackedEntity))
            return;

        var input = EntityManager.GetComponentData<NetcodePlayerInput>(TrackedEntity);
        float2 inputDir = input.inputVector;

        bool isMoving = math.lengthsq(inputDir) > 0.01f;
        if (!isMoving)
        {
            animController.ChangeAnimation(animController.idle);
            return;
        }

        if (inputDir.y > 0.1f)
           animController.ChangeAnimation(animController.walk_forward);
        else if (inputDir.y < -0.1f)
            animController.ChangeAnimation(animController.walk_backward);
        else if (inputDir.x > 0.1f)
            animController.ChangeAnimation(animController.walk_right);
        else if (inputDir.x < -0.1f)
            animController.ChangeAnimation(animController.walk_left);
    }
}
