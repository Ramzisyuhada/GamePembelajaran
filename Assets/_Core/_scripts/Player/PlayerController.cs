using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Jumping,
    Falling,
    Sliding,
    Sitting,
    Sleeping,
    Teleporting
}

public class PlayerController : MonoBehaviour
{
    [Header("Player Status (debug)")]
    [Header("Player Status (debug)")]
    public PlayerState CurrentState = PlayerState.Idle;
    public bool isGrounded = true;
    public bool isOnSlope = false;

    [Header("Idle")]
    [Tooltip("Time before sitting animation plays")]
    public float TimeToSit = 10.0f;

    [Tooltip("Time before sleeping animation plays")]
    public float TimeToSleep = 30.0f;

    [Header("Movement")]
    [Tooltip("Walk speed of the character in m/s")]
    public float WalkSpeed = 2.0f;

    [Tooltip("Run speed of the character in m/s")]
    public float RunSpeed = 4.0f;

    [Tooltip("Time of walking before auto-run")]
    public float TimeToAutoRun = 3.0f;

    [Tooltip("How fast the character accelerates to run")]
    public float RunAcceleration = 2.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Movement FX")]
    [Tooltip("Particle burst played when landing on the ground after a jump or fall.")]
    public ParticleSystem landedStartFx;

    [Tooltip("Particle burst played when sprinting starts while grounded.")]
    public ParticleSystem sprintStartFx;

    [Tooltip("Trail shown while the cat is running on the ground.")]
    public TrailRenderer runTrail;

    [Tooltip("Minimum horizontal speed required to show the run trail.")]
    [Min(0f)] public float runTrailMinSpeed = 2.5f;

    [Tooltip("Seconds of sustained running before the trail appears.")]
    [Min(0f)] public float runTrailActivationDelay = 2f;

    [Header("Spawn")]
    [Tooltip("If true, snap the player to the ActionManager spawn point as soon as it becomes available when the scene starts.")]
    public bool snapToSpawnOnStart = true;

    [Header("Respawn / Teleport")]
    [Tooltip("Time (seconds) to travel back to the spawn point once a killzone is touched.")]
    [Min(0.05f)] public float respawnTravelDuration = 1.5f;

    [Tooltip("If true, pressing the stuck reset key will teleport the player back to the spawn point (only during gameplay).")]
    public bool enableStuckResetKey = true;

    [Tooltip("Key used to teleport back to spawn when stuck.")]
    public Key stuckResetKey = Key.Escape;

    [Tooltip("Maximum height (meters) of the arc travelled during respawn.")]
    [Min(0f)] public float respawnArcHeight = 2f;

    [Tooltip("Curve describing horizontal progress during the respawn travel. X = normalized time, Y = normalized progress.")]
    public AnimationCurve respawnProgressCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Tooltip("Curve describing vertical offset during the respawn travel. X = normalized time, Y = [0,1] height coefficient.")]
    public AnimationCurve respawnHeightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Fall Damage")]
    [Tooltip("Minimum fall height to trigger fall damage event (meters)")]
    public float minFallHeight = 5f;

    [Tooltip("Damage multiplier based on fall height")]
    public float fallDamageMultiplier = 10f;

    [Tooltip("Layer mask for destructible objects")]
    public LayerMask destructibleLayers;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
