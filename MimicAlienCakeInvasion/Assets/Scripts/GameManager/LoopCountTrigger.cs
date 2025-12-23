using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public sealed class LoopCountTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LoopCounter loopCounter;

    [Header("Trigger")]
    [Min(1)]
    [SerializeField] private int threshold = 3;

    [SerializeField] private bool oneShot = true;

    [Header("Events")]
    [SerializeField] private UnityEvent onTriggered;

    private bool _triggered;

    private void Awake()
    {
        if (loopCounter == null)
            loopCounter = FindFirstObjectByType<LoopCounter>();
    }

    private void OnEnable()
    {
        if (loopCounter != null)
            loopCounter.OnLoopChanged += HandleLoopChanged;

        // disparar al habilitarse si ya se cumplió:
        Evaluate(loopCounter != null ? loopCounter.LoopCount : 0);
    }

    private void OnDisable()
    {
        if (loopCounter != null)
            loopCounter.OnLoopChanged -= HandleLoopChanged;
    }

    private void HandleLoopChanged(int newCount)
    {
        Evaluate(newCount);
    }

    private void Evaluate(int loopCount)
    {
        if (_triggered && oneShot)
            return;

        if (loopCount >= threshold)
        {
            _triggered = true;
            Debug.Log($"[LoopCountTrigger] Triggered at loop {loopCount} (threshold {threshold}) on {name}");
            onTriggered?.Invoke();
        }
    }

    // Reseteo manualmente desde otro evento (opcional)
    public void ResetTrigger()
    {
        _triggered = false;
    }
}