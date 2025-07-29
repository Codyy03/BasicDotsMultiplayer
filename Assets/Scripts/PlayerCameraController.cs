using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Entity trackedEntity;
    private EntityManager entityManager;
    void Start()
    {
        if (World.DefaultGameObjectInjectionWorld == null) return;

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        if (trackedEntity == Entity.Null || !entityManager.Exists(trackedEntity))
        {
            Debug.LogWarning("Tracked entity is null or doesn't exist. Disabling camera component.");
            DisableCamera();
            return;
        }

        if (!entityManager.HasComponent<LocalToWorld>(trackedEntity))
        {
            // This is not a local entity - just disable the camera
            DisableCamera();
            return;
        }

        // Local entity - acrivate camera
        ActivateCamera();
    }
    public void ActivateCamera()
    {
        var cam = GetComponent<Camera>();

        if (cam != null)
        {
            cam.enabled = true;
            cam.tag = "MainCamera";
        }
        else
        {
            Debug.LogWarning("No Camera component found on this GameObject.");
        }
    }
    private void DisableCamera()
    {
        var cam = GetComponent<Camera>();
        if (cam != null)
            cam.enabled = false;
    }
}
