using UnityEngine;

[DisallowMultipleComponent]
public sealed class MimicDeathHandler : MonoBehaviour
{
    [SerializeField] private Health health;

    [Header("On Death")]
    [SerializeField] private Collider[] collidersToDisable;
    [SerializeField] private MonoBehaviour[] behavioursToDisable;
    [SerializeField] private bool disableGameObjectOnDeath = true;

    private void Awake()
    {
        if (health == null) health = GetComponentInChildren<Health>();
        if (health != null) health.OnDied += HandleDeath;
    }

    private void OnDestroy()
    {
        if (health != null) health.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        Debug.Log($"[MimicDeathHandler] Mimic died: {name}");

        if (collidersToDisable != null)
            foreach (var c in collidersToDisable)
                if (c) c.enabled = false;

        if (behavioursToDisable != null)
            foreach (var b in behavioursToDisable)
                if (b) b.enabled = false;

        if (disableGameObjectOnDeath)
            gameObject.SetActive(false);
    }
}