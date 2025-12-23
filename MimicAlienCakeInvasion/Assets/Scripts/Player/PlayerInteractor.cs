using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable _focused;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        DetectFocus();
    }

    private void DetectFocus()
    {
        _focused = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableMask))
            return;

        _focused = hit.collider.GetComponentInParent<IInteractable>();
    }

    public void TryInteract()
    {
        if (_focused == null)
            return;

        // Intento: si no se puede, el interactable decide qué feedback dar.
        _focused.Interact();
    }

    // Opcional (útil más adelante para UI): ¿puedo interactuar ahora?
    public bool CanInteractFocused()
    {
        return _focused != null && _focused.CanInteract();
    }
}