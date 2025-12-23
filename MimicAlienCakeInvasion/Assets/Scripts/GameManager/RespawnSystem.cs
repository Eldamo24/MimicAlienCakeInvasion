using UnityEngine;

[DisallowMultipleComponent]
public sealed class RespawnSystem : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private FPMotor motor;
    [SerializeField] private MonoBehaviour inputComponent; 

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        if (playerRoot == null)
            Debug.LogError("[RespawnSystem] PlayerRoot not assigned.");

        if (respawnPoint == null)
            Debug.LogError("[RespawnSystem] RespawnPoint not assigned.");
    }

    public void Respawn(Transform respawnPoint)
    {
        if (respawnPoint == null)
            return;

        playerHealth.ResetHealth();
        motor.ResetMotion();

        bool ccWasEnabled = characterController.enabled;
        characterController.enabled = false;

        playerRoot.position = respawnPoint.position;
        playerRoot.rotation = respawnPoint.rotation;

        characterController.enabled = ccWasEnabled;

        motor.enabled = true;
        inputComponent.enabled = true;
    }
}