using UnityEngine;

public sealed class DamageTester : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private KeyCode key = KeyCode.T;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(key)) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, distance)) return;

        var dmg = hit.collider.GetComponentInParent<IDamageable>();
        if (dmg == null) return;

        dmg.ApplyDamage(new DamageInfo(damage, hit.point, ray.direction, gameObject));
        Debug.Log("[DamageTester] Applied damage");
    }
}