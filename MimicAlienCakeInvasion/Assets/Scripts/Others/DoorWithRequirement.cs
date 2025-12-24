using UnityEngine;

[DisallowMultipleComponent]
public sealed class DoorWithRequirement : MonoBehaviour, IInteractable
{
    [Header("Requirement")]
    [SerializeField] private ItemData requiredItem;

    [Header("References")]
    [SerializeField] private ProgressState progressState;

    [Header("Door")]
    [SerializeField] private bool opensOnce = true;

    private bool _isOpen;

    private void Awake()
    {
        if (progressState == null)
            progressState = FindAnyObjectByType<ProgressState>();
    }

    public bool CanInteract()
    {
        if (_isOpen && opensOnce)
            return false;

        return true;
    }

    public void Interact()
    {
        if (_isOpen && opensOnce)
            return;

        if (requiredItem == null || progressState == null)
        {
            Debug.LogWarning("[DoorWithRequirement] Missing references");
            return;
        }

        if (!progressState.HasLoopItem(requiredItem))
        {
            Debug.Log($"[DoorWithRequirement] Access denied. Missing item: {requiredItem.displayName}");
            return;
        }

        OpenDoor();
    }

    private void OpenDoor()
    {
        _isOpen = true;

        Debug.Log($"[DoorWithRequirement] Door opened using {requiredItem.displayName}");

        // Por ahora:
        // - desactivamos collider
        // - o desactivamos el GameObject
        // (más adelante animación)
        gameObject.SetActive(false);
    }
}