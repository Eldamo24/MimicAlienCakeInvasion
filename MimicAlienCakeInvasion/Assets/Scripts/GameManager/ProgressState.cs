using UnityEngine;

[DisallowMultipleComponent]
public sealed class ProgressState : MonoBehaviour
{
    public bool HasClock { get; private set; }

    public void AcquireClock()
    {
        HasClock = true;
        Debug.Log("[ProgressState] Clock acquired");
    }

    public void ResetProgress()
    {
        HasClock = false;
        Debug.Log("[ProgressState] Progress reset");
    }
}