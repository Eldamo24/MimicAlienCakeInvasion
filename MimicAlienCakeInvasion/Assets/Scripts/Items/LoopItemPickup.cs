using UnityEngine;

[DisallowMultipleComponent]
public sealed class LoopItemPickup : MonoBehaviour, IInteractable, IResettable
{
    private enum ItemState { WorldPickup, HeldUnscanned, Scanned }

    [Header("Item")]
    [SerializeField] private ItemData itemData;

    [Header("References")]
    [SerializeField] private ProgressState progressState;

    private ItemState _state = ItemState.WorldPickup;

    private void Awake()
    {
        if (progressState == null)
            progressState = FindAnyObjectByType<ProgressState>();
    }

    public bool CanInteract()
    {
        if (itemData == null || progressState == null)
            return false;

        return _state switch
        {
            ItemState.WorldPickup => true,
            ItemState.HeldUnscanned => progressState.HasClock,
            ItemState.Scanned => false,
            _ => false
        };
    }

    public void Interact()
    {
        switch (_state)
        {
            case ItemState.WorldPickup:
                Pickup();
                break;
            case ItemState.HeldUnscanned:
                Scan();
                break;
        }
    }

    private void Pickup()
    {
        progressState.AddRunItem(itemData);
        _state = ItemState.HeldUnscanned;
        Debug.Log($"[LoopItemPickup] Picked up (temporary): {itemData.displayName}");
    }

    private void Scan()
    {
        if (!progressState.HasClock)
        {
            Debug.Log("[LoopItemPickup] Cannot scan: no clock");
            return;
        }

        progressState.ScanItem(itemData);
        _state = ItemState.Scanned;
        Debug.Log($"[LoopItemPickup] Scanned and secured: {itemData.displayName}");

        gameObject.SetActive(false);
    }

    // Se llama en cada loop (ResetLevel)
    public void ResetState()
    {
        if (itemData == null || progressState == null)
            return;

        // Si ya está asegurado, este ítem debe seguir "consumido" / fuera del mundo
        if (progressState.HasLoopItem(itemData))
        {
            _state = ItemState.Scanned;
            gameObject.SetActive(false);
            return;
        }

        // Si NO está asegurado, vuelve a estar disponible en el mundo
        _state = ItemState.WorldPickup;
        gameObject.SetActive(true);
    }
}