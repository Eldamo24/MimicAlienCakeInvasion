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
    [SerializeField] private LoopCounter loopCounter;

    [Header("Manual Respawn")]
    [SerializeField] private float manualRespawnCooldown = 0.5f;

    private float _lastManualRespawnTime = -Mathf.Infinity;
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

        progressState.ResetProgress();
        loopManager.ResetLoop();
        loopCounter.ResetCount();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RespawnAtLoop()
    {
        Debug.Log("[RunOrchestrator] Respawning at loop");

        levelResetSystem.ResetLevel();
        respawnSystem.Respawn(loopManager.GetRespawnPoint());
        progressState.ClearRunItemsOnLoop();
        loopCounter.RegisterLoop();

        _handlingDeath = false;
    }

    public void RequestManualRespawn()
    {
        if (!loopManager.IsLoopUnlocked)
            return;

        if (Time.time < _lastManualRespawnTime + manualRespawnCooldown)
            return;

        _lastManualRespawnTime = Time.time;

        Debug.Log("[RunOrchestrator] Manual respawn requested");

        RespawnAtLoop();
    }
}