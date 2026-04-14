using UnityEngine;
using Unity.Mathematics;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public class FirstPersonCharacterController : MonoBehaviour
{
    public Transform ViewEntity;

    public float GroundMaxSpeed = 10f;
    public float GroundedMovementSharpness = 15f;
    public float AirAcceleration = 50f;
    public float AirMaxSpeed = 10f;
    public float AirDrag = 0f;
    public float JumpSpeed = 10f;
    public float3 Gravity = math.up() * -30f;
    public float MinViewAngle = -90f;
    public float MaxViewAngle = 90f;

    CharacterController m_CharacterController;
    Vector3 m_Velocity;
    Vector2 m_MoveInput;
    Vector2 m_LookInput;
    bool m_JumpRequested;
    float m_ViewPitchDegrees;

    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        if (ViewEntity == null)
        {
            Debug.LogError("FirstPersonCharacterController requires a ViewEntity reference.", this);
        }
        GameManager.Instance.SetPlayer(m_CharacterController);
    }

    public void SetInputs(Vector2 moveInput, Vector2 lookInput, bool jumpPressed)
    {
        m_MoveInput = Vector2.ClampMagnitude(moveInput, 1f);
        m_LookInput = lookInput;
        m_JumpRequested |= jumpPressed;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + m_LookInput.x, 0f);

        m_ViewPitchDegrees = Mathf.Clamp(m_ViewPitchDegrees + m_LookInput.y, MinViewAngle, MaxViewAngle);
        if (ViewEntity != null)
        {
            ViewEntity.localRotation = Quaternion.AngleAxis(m_ViewPitchDegrees, Vector3.left);
        }

        m_LookInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        if (m_CharacterController.isGrounded && m_Velocity.y < 0f)
        {
            m_Velocity.y = -2f;
        }

        Vector3 desiredMove = (transform.right * m_MoveInput.x) + (transform.forward * m_MoveInput.y);
        desiredMove = Vector3.ClampMagnitude(desiredMove, 1f);

        Vector3 horizontalVelocity = new Vector3(m_Velocity.x, 0f, m_Velocity.z);
        if (m_CharacterController.isGrounded)
        {
            Vector3 targetVelocity = desiredMove * GroundMaxSpeed;
            float sharpness = 1f - Mathf.Exp(-GroundedMovementSharpness * deltaTime);
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, sharpness);
        }
        else
        {
            horizontalVelocity += desiredMove * (AirAcceleration * deltaTime);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, AirMaxSpeed);
            horizontalVelocity /= 1f + (AirDrag * deltaTime);
        }

        m_Velocity.x = horizontalVelocity.x;
        m_Velocity.z = horizontalVelocity.z;

        if (m_JumpRequested && m_CharacterController.isGrounded)
        {
            m_Velocity.y = JumpSpeed;
        }

        m_Velocity += (Vector3)Gravity * deltaTime;
        m_CharacterController.Move(m_Velocity * deltaTime);
        m_JumpRequested = false;
    }
}
