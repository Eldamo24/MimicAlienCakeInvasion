using UnityEngine;

[DisallowMultipleComponent]
public sealed class LoopTerminal : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private ProgressState progressState;
    [SerializeField] private LoopManager loopManager;

    [Header("Loop")]
    [SerializeField] private Transform loopPoint;

    private bool _isActivated;

    private void Awake()
    {
        if (progressState == null)
            progressState = FindFirstObjectByType<ProgressState>();

        if (loopManager == null)
            loopManager = FindFirstObjectByType<LoopManager>();

        if (loopPoint == null)
            loopPoint = transform;
    }

    public bool CanInteract()
    {
        if (_isActivated)
            return false;

        if (progressState == null || !progressState.HasClock)
            return false;

        return true;
    }

    public void Interact()
    {
        if (_isActivated)
        {
            Debug.Log("[LoopTerminal] Already activated.");
            return;
        }

        if (progressState == null || !progressState.HasClock)
        {
            Debug.Log("[LoopTerminal] You need a clock to activate this terminal.");
            return;
        }

        loopManager.ActivateLoopPoint(loopPoint);
        _isActivated = true;

        Debug.Log($"[LoopTerminal] Activated terminal {name}");
    }
}