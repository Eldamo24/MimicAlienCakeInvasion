using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class LoopCounter : MonoBehaviour
{
    public int LoopCount { get; private set; }

    public event Action<int> OnLoopChanged;

    private void Awake()
    {
        LoopCount = 0;
    }

    public void RegisterLoop()
    {
        LoopCount++;
        Debug.Log($"[LoopCounter] Loop registered. Count = {LoopCount}");
        OnLoopChanged?.Invoke(LoopCount);
    }

    public void ResetCount()
    {
        LoopCount = 0;
        Debug.Log("[LoopCounter] Loop count reset");
        OnLoopChanged?.Invoke(LoopCount);
    }
}