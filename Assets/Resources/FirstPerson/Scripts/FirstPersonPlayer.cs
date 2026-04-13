using System;
using UnityEngine;

[Serializable]
public struct FirstPersonPlayerSettings
{
    public FirstPersonCharacterController ControlledCharacter;
    public float LookInputSensitivity;
}

[Serializable]
public struct FirstPersonPlayerInputs
{
    public Vector2 MoveInput;
    public Vector2 LookInput;
    public bool JumpPressed;
}
