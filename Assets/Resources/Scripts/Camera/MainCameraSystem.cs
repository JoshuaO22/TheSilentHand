using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class MainCameraSystem : MonoBehaviour
{
    public Transform Target;
    public Camera cameraInstance;

    void LateUpdate()
    {
        if (cameraInstance == null)
        {
            return;
        }

        Transform cameraTarget = Target;

        if (cameraTarget != null)
        {
            cameraInstance.transform.SetPositionAndRotation(cameraTarget.position, cameraTarget.rotation);
        }
    }
}
