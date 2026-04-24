// =============================================================================
// CameraFollow.cs
// Smoothly follows a target along the X axis while keeping Y fixed.
// =============================================================================

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Target")]
    public Transform target;

    [Header("Follow")]
    public float followSpeed = 5f;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private float offsetX = 0f;
    private float fixedY = 0f;
    private float startX = 0f;
    private bool followEnabled = true;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Cache the initial offset and fixed Y position relative to the target
    void Start()
    {
        if (target == null)
        {
            return;
        }

        startX = transform.position.x;
        offsetX = startX - target.position.x;
        fixedY = transform.position.y;
    }

    // Lerp the camera X toward the target each frame
    void Update()
    {
        if (target == null || !followEnabled)
        {
            return;
        }

        float targetCameraX = target.position.x + offsetX;
        float newX = Mathf.Lerp(transform.position.x, targetCameraX, Time.deltaTime * followSpeed);

        transform.position = new Vector3(newX, fixedY, transform.position.z);
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    // Enable or disable camera following
    public void SetFollowEnabled(bool enabled)
    {
        followEnabled = enabled;
    }

    // Re-enable following and snap the camera back to its starting position
    public void ResetToStart()
    {
        followEnabled = true;
        transform.position = new Vector3(startX, fixedY, transform.position.z);
    }
}