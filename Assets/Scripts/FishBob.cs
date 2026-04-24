// =============================================================================
// FishBob.cs
// Applies a looping vertical bob and horizontal wobble to a fish GameObject.
// =============================================================================

using UnityEngine;

public class FishBob : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Bob")]
    public float bobAmplitude = 5f;
    public float bobSpeed = 0.5f;

    [Header("Wobble")]
    public float wobbleAmplitude = 3f;
    public float wobbleSpeed = 0.3f;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private float bobTime = 0f;
    private float baseX = 0f;
    private float baseY = 0f;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Capture starting position and randomise phase so fish don't all move in sync
    void Start()
    {
        baseX = transform.position.x;
        baseY = transform.position.y;
        bobTime = Random.Range(0f, Mathf.PI * 2f);
    }

    // Apply vertical bob and horizontal wobble each frame
    void Update()
    {
        bobTime += Time.deltaTime;

        float newY = baseY + Mathf.Sin(bobTime * bobSpeed * Mathf.PI * 2f) * bobAmplitude;
        float newX = baseX + Mathf.Sin(bobTime * wobbleSpeed * Mathf.PI * 2f) * wobbleAmplitude;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}