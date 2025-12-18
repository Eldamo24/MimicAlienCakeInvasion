using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public sealed class FPMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform moveBasis;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float runSpeed = 7.0f;
    [SerializeField] private float acceleration = 22f;
    [SerializeField] private float deceleration = 26f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 1.25f;
    [SerializeField] private float gravity = -22f;
    [SerializeField] private float groundedStickForce = -2.0f;
    [SerializeField] private float coyoteTime = 0.10f;
    [SerializeField] private float jumpBuffer = 0.10f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _characterController;

    private Vector2 _moveInput;
    private bool _runHeld;

    private Vector3 _horizontalVelocity;
    private float _verticalVelocity;

    private float _timeSinceGrounded;
    private float _timeSinceJumpPressed;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if (moveBasis == null) moveBasis = transform;
        _timeSinceJumpPressed = float.PositiveInfinity;
        _timeSinceGrounded = float.PositiveInfinity;
        _verticalVelocity = 0f;
    }

    private void Update()
    {
        UpdateGrounding();
        UpdateJumpTimers();
        UpdateHorizontalMotion();
        UpdateVerticalMotion();

        Vector3 velocity = _horizontalVelocity + Vector3.up * _verticalVelocity;
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateGrounding()
    {
        bool ccGrounded = _characterController.isGrounded;

        Vector3 origin = transform.position + _characterController.center;
        float castDistance = (_characterController.height * 0.5f) + groundCheckDistance;

        bool rayGrounded = Physics.SphereCast(
            origin,
            _characterController.radius * 0.95f,
            Vector3.down,
            out _,
            castDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        IsGrounded = ccGrounded || rayGrounded;

        if (IsGrounded) _timeSinceGrounded = 0f;
        else _timeSinceGrounded += Time.deltaTime;
    }

    private void UpdateJumpTimers()
    {
        _timeSinceJumpPressed += Time.deltaTime;

        bool bufferedJump = _timeSinceJumpPressed <= jumpBuffer;
        bool canCoyote = _timeSinceGrounded <= coyoteTime;

        if (bufferedJump && canCoyote)
        {
            // Consumir buffer + coyote
            _timeSinceJumpPressed = float.PositiveInfinity;
            _timeSinceGrounded = float.PositiveInfinity;

            // v = sqrt(2gh)
            _verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
            IsGrounded = false;
        }
    }

    private void UpdateHorizontalMotion()
    {
        float targetSpeed = _runHeld ? runSpeed : walkSpeed;

        Vector3 inputWorld =
            (moveBasis.right * _moveInput.x) +
            (moveBasis.forward * _moveInput.y);

        inputWorld.y = 0f;
        inputWorld = inputWorld.sqrMagnitude > 0f ? inputWorld.normalized : Vector3.zero;

        Vector3 targetVelocity = inputWorld * targetSpeed;
        float accel = (inputWorld.sqrMagnitude > 0f) ? acceleration : deceleration;

        _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, accel * Time.deltaTime);
    }

    private void UpdateVerticalMotion()
    {
        // Mantener pegado al suelo sin forzar Move() adicional
        if (IsGrounded && _verticalVelocity < 0f)
            _verticalVelocity = groundedStickForce;

        _verticalVelocity += gravity * Time.deltaTime;
    }

    public void SetMoveInput(Vector2 move) => _moveInput = Vector2.ClampMagnitude(move, 1f);
    public void SetRunHeld(bool runHeld) => _runHeld = runHeld;

    public void PressJump()
    {
        // Jump buffer
        _timeSinceJumpPressed = 0f;
    }
}