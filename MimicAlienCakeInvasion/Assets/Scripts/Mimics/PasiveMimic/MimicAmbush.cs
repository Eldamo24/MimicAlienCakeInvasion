using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class MimicAmbush : MonoBehaviour, IResettable
{
    [Header("Damage")]
    [SerializeField] private float attackCooldown;

    [Header("Reveal")]
    [SerializeField] private bool revealOnce = true;

    private bool _isRevealed;

    private IHitReceiver _currentTarget;
    private Transform _targetRoot;

    private SphereCollider _trigger;
    private Coroutine _attackLoop;

    private void Awake()
    {
        _trigger = GetComponent<SphereCollider>();
        if (_trigger == null || !_trigger.isTrigger)
        {
            Debug.LogError("[MimicAmbush] Requires a SphereCollider set as Trigger.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var receiver = other.GetComponentInParent<IHitReceiver>();
        if (receiver == null) return;

        _currentTarget = receiver;
        _targetRoot = other.transform.root;

        if (!_isRevealed)
            Reveal();

        if (_attackLoop == null)
            _attackLoop = StartCoroutine(AttackLoop());
    }

    private void OnTriggerExit(Collider other)
    {
        var receiver = other.GetComponentInParent<IHitReceiver>();
        if (receiver == null) return;
        if (receiver != _currentTarget) return;

        ClearTarget();
    }

    private IEnumerator AttackLoop()
    {
        TryApplyHit();
        while (_currentTarget != null)
        {
            yield return new WaitForSeconds(attackCooldown);

            if (!IsTargetStillInRange())
            {
                ClearTarget();
                yield break;
            }

            TryApplyHit();
        }
    }

    private void TryApplyHit()
    {
        if (_currentTarget == null)
            return;
        _currentTarget.ApplyHit();
    }

    private bool IsTargetStillInRange()
    {
        if (_targetRoot == null)
            return false;

        Vector3 center = _trigger.transform.position;
        float radius = _trigger.radius * Mathf.Max(
            _trigger.transform.lossyScale.x,
            _trigger.transform.lossyScale.y,
            _trigger.transform.lossyScale.z
        );
        float distance = Vector3.Distance(center, _targetRoot.position);
        return distance <= radius;
    }

    private void ClearTarget()
    {
        if (_attackLoop != null)
        {
            StopCoroutine(_attackLoop);
            _attackLoop = null;
        }

        _currentTarget = null;
        _targetRoot = null;
    }

    private void Reveal()
    {
        _isRevealed = true;
        Debug.Log("Mimic revealed!");
        if (revealOnce)
        {
            // queda revelado para siempre
        }
    }

    public void ResetState()
    {
        _isRevealed = false;
        ClearTarget();
        // volver a estado “disfrazado”
    }
}