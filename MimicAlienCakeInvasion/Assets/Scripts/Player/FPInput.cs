using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public sealed class FPInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FPMotor motor;
    [SerializeField] private FPLook look;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;

    private bool _isMouseLookSource = true;

    private void Awake()
    {
        if (motor == null) motor = GetComponent<FPMotor>();
        if (look == null) look = GetComponentInChildren<FPLook>();

        Debug.Assert(moveAction != null, "Move Action Reference missing");
        Debug.Assert(lookAction != null, "Look Action Reference missing");
        Debug.Assert(jumpAction != null, "Jump Action Reference missing");
        Debug.Assert(sprintAction != null, "Sprint Action Reference missing");
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();

        jumpAction.action.performed += OnJumpPerformed;
        sprintAction.action.performed += OnSprintPressed;
        sprintAction.action.canceled += OnSprintCanceled;

        lookAction.action.performed += OnLookSourceChanged;
        lookAction.action.canceled += OnLookSourceChanged;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJumpPerformed;
        sprintAction.action.performed -= OnSprintPressed;
        sprintAction.action.canceled -= OnSprintCanceled;

        lookAction.action.performed -= OnLookSourceChanged;
        lookAction.action.canceled -= OnLookSourceChanged;

        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
    }

    private void Update()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>();
        motor.SetMoveInput(move);

        Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();
        look.ApplyLook(lookDelta, _isMouseLookSource);
    }

    private void OnJumpPerformed(InputAction.CallbackContext _)
    {
        motor.PressJump();
    }

    private void OnSprintPressed(InputAction.CallbackContext _)
    {
        motor.SetRunHeld(true);
    }

    private void OnSprintCanceled(InputAction.CallbackContext _)
    {
        motor.SetRunHeld(false);
    }

    private void OnLookSourceChanged(InputAction.CallbackContext ctx)
    {
        _isMouseLookSource = ctx.control?.device is Mouse;
    }
}