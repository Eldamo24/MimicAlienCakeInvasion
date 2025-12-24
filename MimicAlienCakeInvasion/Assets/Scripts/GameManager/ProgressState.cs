using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class ProgressState : MonoBehaviour
{
    // ===== Clock =====
    public bool HasClock { get; private set; }

    // ===== Inventories =====
    private readonly HashSet<ItemData> _runItems = new();
    private readonly HashSet<ItemData> _loopItems = new();

    // ===== Clock =====
    public void AcquireClock()
    {
        HasClock = true;
        Debug.Log("[ProgressState] Clock acquired");
    }

    // ===== Run Items =====
    public void AddRunItem(ItemData item)
    {
        if (item == null) return;

        if (_runItems.Add(item))
        {
            Debug.Log($"[ProgressState] Run item added: {item.displayName}");
        }
    }

    public bool HasRunItem(ItemData item)
    {
        return item != null && _runItems.Contains(item);
    }

    // ===== Loop Items (Scanned) =====
    public bool HasLoopItem(ItemData item)
    {
        return item != null && _loopItems.Contains(item);
    }

    public void ScanItem(ItemData item)
    {
        if (item == null) return;

        if (!_runItems.Contains(item))
        {
            Debug.LogWarning($"[ProgressState] Cannot scan item not in run: {item.displayName}");
            return;
        }

        _runItems.Remove(item);
        _loopItems.Add(item);

        Debug.Log($"[ProgressState] Item scanned and secured: {item.displayName}");
    }

    // ===== Loop Handling =====
    public void ClearRunItemsOnLoop()
    {
        if (_runItems.Count > 0)
        {
            Debug.Log("[ProgressState] Clearing run items on loop");
            _runItems.Clear();
        }
    }

    // ===== Full Reset =====
    public void ResetProgress()
    {
        HasClock = false;
        _runItems.Clear();
        _loopItems.Clear();

        Debug.Log("[ProgressState] Full progress reset");
    }
}