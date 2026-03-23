using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class MainCameraSystem : SystemBase
{
    // TODO: Fix MainGameObjectCamera's Instance to remove the fallback
    Camera Instance = MainGameObjectCamera.Instance ?? GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    protected override void OnUpdate()
    {
        if (Instance != null && SystemAPI.HasSingleton<MainEntityCamera>())
        {
            Entity mainEntityCameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            LocalToWorld targetLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(mainEntityCameraEntity);
            Instance.transform.SetPositionAndRotation(targetLocalToWorld.Position, targetLocalToWorld.Rotation);
        }
    }
}
