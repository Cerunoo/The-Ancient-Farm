using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private ScriptableStats _stats;

    // [Header("States")]
    // ... some states

    private FrameInput _frameInput;

    private Rigidbody2D _rb;

    private Vector2 _frameVelocity;

    #region Interface

    public Vector2 FrameDirection => new Vector2(_frameInput.Horizontal, _frameInput.Vertical);

    #endregion

    private void Awake()
    {
        Instance = this;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            Horizontal = Input.GetAxisRaw("Horizontal"),
            Vertical = Input.GetAxisRaw("Vertical"),

            IsCreep = Input.GetKey(KeyCode.LeftShift)
        };

        if (_stats.SnapInput)
        {
            _frameInput.Horizontal = Mathf.Abs(_frameInput.Horizontal) < _stats.HorizontalDeadZoneThreshold ? 0 : _frameInput.Horizontal;
            _frameInput.Vertical = Mathf.Abs(_frameInput.Vertical) < _stats.VerticalDeadZoneThreshold ? 0 : _frameInput.Vertical;
        }
    }

    private void FixedUpdate()
    {
        HandleDirection();

        ApplyMovement();
    }

    #region Direction

    private void HandleDirection()
    {
        if (_frameInput.Horizontal == 0)
        {
            var deceleration = _stats.Deceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            float speed = _stats.MaxSpeed;;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Horizontal * speed, _stats.Acceleration * Time.fixedDeltaTime);
        }

        if (_frameInput.Vertical == 0)
        {
            var deceleration = _stats.Deceleration;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            float speed = _stats.MaxSpeed;;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, _frameInput.Vertical * speed, _stats.Acceleration * Time.fixedDeltaTime);
        }

        if (_frameInput.Horizontal != 0 && _frameInput.Vertical != 0)
        {
            _frameVelocity.x *= _stats.moveDiagonalLimiter;
            _frameVelocity.y *= _stats.moveDiagonalLimiter;
        }

        if (_frameInput.IsCreep)
        {
            _frameVelocity.x *= _stats.CreepSpeedMultiplier;
            _frameVelocity.y *= _stats.CreepSpeedMultiplier;
        }
    }

    #endregion

    private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;
}

public struct FrameInput
{
    public float Horizontal;
    public float Vertical;

    public bool IsCreep;
}

public interface IPlayerController
{
    public Vector2 FrameDirection { get; }
}