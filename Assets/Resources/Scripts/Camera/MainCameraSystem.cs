using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class MainCameraSystem : MonoBehaviour
{
    private Camera cameraInstance;
    private GameObject player;

    void Awake()
    {
        FindObjects();
    }

    void LateUpdate()
    {
        if (cameraInstance == null)
        {
            Debug.LogError("MainCamera not found in the scene. Please ensure there is a GameObject tagged 'MainCamera' with a Camera component.");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player with CharacterController not found in the scene. Please ensure there is a GameObject with a CharacterController component.");
            return;
        }

        Transform cameraTarget = player.transform.Find("View");

        if (cameraTarget != null && player != null && player.GetComponent<CharacterController>() != null && player.GetComponent<CharacterController>().enabled)
        {
            transform.SetPositionAndRotation(cameraTarget.position, cameraTarget.rotation);
        }
    }

    private void FindObjects()
    {
        if (cameraInstance == null)
        {
            cameraInstance = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            if (cameraInstance == null)
            {
                Debug.LogError("MainCamera not found in the scene. Please ensure there is a GameObject tagged 'MainCamera' with a Camera component.");
            }
        }

        if (player == null)
        {
            player = FindAnyObjectByType<CharacterController>()?.gameObject;
            if (player == null)
            {
                Debug.LogError("Player with CharacterController not found in the scene. Please ensure there is a GameObject with a CharacterController component.");
            }
        }
    }
}
