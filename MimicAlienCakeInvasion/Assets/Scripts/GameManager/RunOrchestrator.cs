using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public sealed class RunOrchestrator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LoopManager loopManager;
    [SerializeField] private ProgressState progressState;
    [SerializeField] private RespawnSystem respawnSystem;
    [SerializeField] private LevelResetSystem levelResetSystem;

    private bool _handlingDeath;

    public void HandlePlayerDeath()
    {
        if (_handlingDeath)
            return;

        _handlingDeath = true;

        if (!loopManager.IsLoopUnlocked)
        {
            RestartRun();
        }
        else
        {
            RespawnAtLoop();
        }
    }

    private void RestartRun()
    {
        Debug.Log("[RunOrchestrator] Restarting run");

        // Reset explícito de progreso
        progressState.ResetProgress();
        loopManager.ResetLoop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RespawnAtLoop()
    {
        Debug.Log("[RunOrchestrator] Respawning at loop");

        // Resetear mundo reseteable
        levelResetSystem.ResetLevel();

        // Respawn usando el loop actual
        respawnSystem.Respawn(loopManager.GetRespawnPoint());
        _handlingDeath = false;
    }
}