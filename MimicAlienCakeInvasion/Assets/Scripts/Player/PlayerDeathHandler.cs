using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerDeathHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth health;
    [SerializeField] private FPMotor motor;
    [SerializeField] private MonoBehaviour inputComponent; 

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] private RunOrchestrator runOrchestrator;

    private bool _isDead;

    private void Awake()
    {
        if (health == null)
            health = GetComponent<PlayerHealth>();

        if (motor == null)
            motor = GetComponent<FPMotor>();
    }

    private void OnEnable()
    {
        health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (_isDead)
            return;

        _isDead = true;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // Bloquear control
        SetControlEnabled(false);

        // Disparar feedback (fade, sonido, etc.)
        yield return new WaitForSeconds(respawnDelay);

        runOrchestrator.HandlePlayerDeath();

        _isDead = false;
    }

    private void SetControlEnabled(bool enabled)
    {
        if (motor != null)
            motor.enabled = enabled;

        if (inputComponent != null)
            inputComponent.enabled = enabled;
    }
}