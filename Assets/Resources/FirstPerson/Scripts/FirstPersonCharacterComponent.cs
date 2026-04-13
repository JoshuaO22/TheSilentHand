using System;
using UnityEngine;
using Unity.Mathematics;

[Serializable]
public struct FirstPersonCharacterComponent
{
    public float GroundMaxSpeed;
    public float GroundedMovementSharpness;
    public float AirAcceleration;
    public float AirMaxSpeed;
    public float AirDrag;
    public float JumpSpeed;
    public float3 Gravity;

    public float MinViewAngle;
    public float MaxViewAngle;

    public Transform ViewEntity;
    public float ViewPitchDegrees;
    public Quaternion ViewLocalRotation;
}

[Serializable]
public struct FirstPersonCharacterControl
{
    public Vector3 MoveVector;
    public Vector2 LookDegreesDelta;
    public bool Jump;
}

[Serializable]
public struct FirstPersonCharacterView
{
    public FirstPersonCharacterController Character;
}
