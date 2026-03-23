using UnityEngine;

public class MainGameObjectCamera : MonoBehaviour
{
    public static Camera Instance;

    void Awake()
    {
        // TODO: This doesn't always find the camera and set it to the Instance.
        Instance = GetComponent<Camera>();
    }
}
