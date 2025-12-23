using UnityEngine;

[DisallowMultipleComponent]
public sealed class LoopManager : MonoBehaviour
{
    [Header("Defaults")]
    [SerializeField] private Transform defaultStartPoint;

    public bool IsLoopUnlocked { get; private set; }
    public Transform CurrentLoopPoint { get; private set; }

    private void Awake()
    {
        CurrentLoopPoint = defaultStartPoint;
        IsLoopUnlocked = false;
    }

    public void ActivateLoopPoint(Transform loopPoint)
    {
        if (loopPoint == null)
            return;

        CurrentLoopPoint = loopPoint;
        IsLoopUnlocked = true;

        Debug.Log($"[LoopManager] Loop activated at {loopPoint.name}");
    }

    public Transform GetRespawnPoint()
    {
        return CurrentLoopPoint;
    }

    public void ResetLoop()
    {
        CurrentLoopPoint = defaultStartPoint;
        IsLoopUnlocked = false;

        Debug.Log("[LoopManager] Loop reset to default start");
    }
}