using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class Health : MonoBehaviour, IDamageable
{
    [Header("HP")]
    [SerializeField] private float maxHp = 10f;

    [Header("Invulnerability")]
    [SerializeField] private float invulnTime = 0.0f;

    public float MaxHp => maxHp;
    public float CurrentHp { get; private set; }
    public bool IsAlive { get; private set; } = true;

    public event Action<DamageInfo> OnDamaged;
    public event Action OnDied;

    private float _invulnUntil;

    private void Awake()
    {
        CurrentHp = maxHp;
        IsAlive = true;
    }

    public void ResetFull()
    {
        CurrentHp = maxHp;
        IsAlive = true;
        _invulnUntil = 0f;
    }

    public void ApplyDamage(DamageInfo info)
    {
        if (!IsAlive) return;

        if (invulnTime > 0f && Time.time < _invulnUntil)
            return;

        CurrentHp -= info.Amount;
        _invulnUntil = Time.time + invulnTime;

        OnDamaged?.Invoke(info);

        if (CurrentHp <= 0f)
        {
            IsAlive = false;
            CurrentHp = 0f;
            OnDied?.Invoke();
        }
    }
}