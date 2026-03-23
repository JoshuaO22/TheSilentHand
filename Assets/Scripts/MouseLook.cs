using UnityEngine;

public class MouseLook : MonoBehaviour
{
    void Start()
    {
        // Lock the cursor to the center of the screen to prevent distraction
        Cursor.lockState = CursorLockMode.Locked;
    }
}