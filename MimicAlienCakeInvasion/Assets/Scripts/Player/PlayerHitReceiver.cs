using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerHitReceiver : MonoBehaviour, IHitReceiver
{
    [SerializeField] private PlayerHealth health;
    private void Awake()
    {
        if (health == null)
            health = GetComponent<PlayerHealth>();
    }

    public bool ApplyHit()
    {
        return health != null && health.ApplyHit();
    }
}