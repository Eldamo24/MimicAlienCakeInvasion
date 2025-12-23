using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public sealed class FPInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FPMotor motor;
    [SerializeField] private FPLook look;
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private RunOrchestrator runOrchestrator;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private InputActionReference manualRespawn;

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
        interact.action.Enable();
        manualRespawn.action.Enable();

        jumpAction.action.performed += OnJumpPerformed;
        sprintAction.action.performed += OnSprintPressed;
        sprintAction.action.canceled += OnSprintCanceled;
        interact.action.performed += OnInteract;
        manualRespawn.action.performed += OnManualRespawn;

        lookAction.action.performed += OnLookSourceChanged;
        lookAction.action.canceled += OnLookSourceChanged;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJumpPerformed;
        sprintAction.action.performed -= OnSprintPressed;
        sprintAction.action.canceled -= OnSprintCanceled;
        interact.action.performed -= OnInteract;
        manualRespawn.action.performed -= OnManualRespawn;

        lookAction.action.performed -= OnLookSourceChanged;
        lookAction.action.canceled -= OnLookSourceChanged;

        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        interact.action.Disable();
        manualRespawn.action.Disable();
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

    private void OnInteract(InputAction.CallbackContext _)
    {
        interactor.TryInteract();
    }

    private void OnManualRespawn(InputAction.CallbackContext _)
    {
        runOrchestrator.RequestManualRespawn();
    }
}