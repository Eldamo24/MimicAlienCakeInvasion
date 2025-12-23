using UnityEngine;

[DisallowMultipleComponent]
public sealed class ClockPickUp : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private ProgressState progressState;

    private void Awake()
    {
        if (progressState == null)
            progressState = FindFirstObjectByType<ProgressState>();
    }

    public bool CanInteract()
    {
        return progressState != null && !progressState.HasClock;
    }

    public void Interact()
    {
        if (progressState == null)
            return;
        progressState.AcquireClock();
        // Por ahora solo se desactiva
        gameObject.SetActive(false);
    }
}