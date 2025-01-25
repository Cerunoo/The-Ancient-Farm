using UnityEngine;

[CreateAssetMenu]
public class ScriptableStats : ScriptableObject
{
    [Header("INPUT")]
    [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
    public bool SnapInput = true;

    [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
    public float HorizontalDeadZoneThreshold = 0.1f;

    [Tooltip("Minimum input required before a down or up is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
    public float VerticalDeadZoneThreshold = 0.1f;

    [Header("MOVEMENT")]
    [Tooltip("Constant speed")]
    public float MaxSpeed = 14;

    [Tooltip("Creep Multiplier for speed")]
    public float CreepSpeedMultiplier = 0.6f;

    [Tooltip("Move Diagonally Limiter. Recommend value = 70%")]
    public float moveDiagonalLimiter = 0.7f;

    [Tooltip("The player's capacity to gain horizontal speed")]
    public float Acceleration = 120;

    [Tooltip("The pace at which the player comes to a stop")]
    public float Deceleration = 60;
}