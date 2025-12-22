using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerHealth : MonoBehaviour
{
    [Header("Health (Hits)")]
    [SerializeField] private int maxHits;
    [SerializeField] private float invulnerabilitySeconds;

    public int CurrentHits { get; private set; }
    public bool IsAlive => CurrentHits > 0;

    public event Action<int> OnDamaged;
    public event Action OnDied;

    private float _invulnTimer;
    private bool _isInvulnerable;

    private void Awake()
    {
        ResetHealth();
    }

    private void Update()
    {
        if (!_isInvulnerable)
            return;

        _invulnTimer -= Time.deltaTime;
        if (_invulnTimer <= 0f)
            _isInvulnerable = false;
    }

    /// <summary>
    /// Aplica un hit. Retorna true si el hit fue aplicado.
    /// </summary>
    public bool ApplyHit()
    {
        if (!IsAlive)
            return false;

        if (_isInvulnerable)
            return false;

        CurrentHits = Mathf.Max(0, CurrentHits - 1);
        OnDamaged?.Invoke(CurrentHits);

        if (CurrentHits <= 0)
        {
            OnDied?.Invoke();
        }
        else
        {
            StartInvulnerability();
        }

        return true;
    }

    public void ResetHealth()
    {
        CurrentHits = maxHits;
        _isInvulnerable = false;
        _invulnTimer = 0f;
    }

    private void StartInvulnerability()
    {
        _isInvulnerable = true;
        _invulnTimer = invulnerabilitySeconds;
    }
}