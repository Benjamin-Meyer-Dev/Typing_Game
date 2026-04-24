// =============================================================================
// SubmarineCollision.cs
// Detects fish collisions and triggers a blink effect and life loss.
// =============================================================================

using UnityEngine;
using System.Collections;

public class SubmarineCollision : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("References")]
    public TypingGame typingGame;

    [Header("Blink Settings")]
    public int blinkCount = 3;
    public float blinkInterval = 0.15f;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private SpriteRenderer spriteRenderer;
    private bool isBlinking = false;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Cache the sprite renderer on startup
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Notify the game of a fish hit and start the blink effect
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && !isBlinking)
        {
            typingGame.OnHitFish();
            StartCoroutine(BlinkEffect());
        }
    }

    // -------------------------------------------------------------------------
    // Effects
    // -------------------------------------------------------------------------

    // Toggle the sprite on and off for the configured number of blinks
    private IEnumerator BlinkEffect()
    {
        isBlinking = true;

        for (int index = 0; index < blinkCount; index++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }

        isBlinking = false;
    }
}