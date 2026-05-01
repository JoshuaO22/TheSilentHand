using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[DisallowMultipleComponent]
public class FirstPersonInputController : MonoBehaviour
{
    public FirstPersonCharacterController ControlledCharacter;
    public float LookInputSensitivity = 0.2f;

    private InputAction lookAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        lookAction.Enable();
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (lookAction == null || moveAction == null || jumpAction == null || sprintAction == null)
        {
            return;
        }

        Vector2 moveInput = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);

        Vector2 lookInput = Vector2.ClampMagnitude(lookAction.ReadValue<Vector2>() * LookInputSensitivity, 50f);

        bool jumpPressed = jumpAction.WasPressedThisFrame();
        bool sprintHeld = sprintAction.IsPressed();
#else
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 lookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * LookInputSensitivity;
        bool jumpPressed = Input.GetButtonDown("Jump");
        bool sprintHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
#endif

        if (ControlledCharacter != null)
        {
            ControlledCharacter.SetInputs(moveInput, lookInput, jumpPressed, sprintHeld);
        }
    }
}
