using UnityEngine;

[DisallowMultipleComponent]
public sealed class FPLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform yawTarget;   
    [SerializeField] private Transform pitchTarget; 

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float stickSensitivity = 140f; 
    [SerializeField] private float pitchMin = -85f;
    [SerializeField] private float pitchMax = 85f;

    private float _pitch;

    private void Awake()
    {
        if (yawTarget == null) yawTarget = transform;
        if (pitchTarget == null) pitchTarget = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ApplyLook(Vector2 lookDelta, bool isMouse)
    {
        float yawDelta;
        float pitchDelta;

        if (isMouse)
        {
            yawDelta = lookDelta.x * mouseSensitivity;
            pitchDelta = lookDelta.y * mouseSensitivity;
        }
        else
        {
            yawDelta = lookDelta.x * stickSensitivity * Time.deltaTime;
            pitchDelta = lookDelta.y * stickSensitivity * Time.deltaTime;
        }

        yawTarget.Rotate(Vector3.up, yawDelta, Space.World);

        _pitch -= pitchDelta;
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);
        pitchTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}