using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[DisallowMultipleComponent]
public class FirstPersonInputController : MonoBehaviour
{
    public FirstPersonCharacterController ControlledCharacter;
    public float LookInputSensitivity = 0.2f;

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null || Mouse.current == null)
        {
            return;
        }

        Vector2 moveInput = new Vector2(
            (Keyboard.current.dKey.isPressed ? 1f : 0f) + (Keyboard.current.aKey.isPressed ? -1f : 0f),
            (Keyboard.current.wKey.isPressed ? 1f : 0f) + (Keyboard.current.sKey.isPressed ? -1f : 0f));

        Vector2 lookInput = Mouse.current.delta.ReadValue() * LookInputSensitivity;
        bool jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 lookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * LookInputSensitivity;
        bool jumpPressed = Input.GetButtonDown("Jump");
#endif

        if (ControlledCharacter != null)
        {
            ControlledCharacter.SetInputs(moveInput, lookInput, jumpPressed);
        }
    }    
}
