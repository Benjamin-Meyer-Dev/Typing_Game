// =============================================================================
// SubmarineController.cs
// Controls submarine row sliding, forward movement, bobbing, and victory loops.
// =============================================================================

using UnityEngine;
using System.Collections;

public class SubmarineController : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Rows")]
    public float[] rowYPositions = new float[3] { -345f, -250f, -155f };

    [Header("Slide")]
    public float slideSpeed = 10f;

    [Header("Bob")]
    public float bobAmplitude = 5f;
    public float bobSpeed = 0.5f;

    [Header("Forward Movement")]
    public float moveStepX = 400f;
    public float forwardSlideSpeed = 5f;

    [Header("Victory")]
    public float victorySlideDuration = 0.4f;
    public float loopRadiusX = 160f;
    public float loopRadiusY = 80f;
    public float loopCycleDuration = 1.8f;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private int currentRowIndex = 1;

    private float currentY = 0f;
    private float targetY = 0f;

    private float currentX = 0f;
    private float targetX = 0f;
    private float startX = 0f;

    private float bobTime = 0f;

    private bool victoryMode = false;
    private Coroutine victoryCoroutine;
    private Vector3 originalScale;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Cache original scale and snap to the starting row
    void Start()
    {
        originalScale = transform.localScale;

        startX = transform.position.x;
        currentX = startX;
        targetX = startX;

        SnapToRow(currentRowIndex);
    }

    // Drive normal submarine movement when not in victory mode
    void Update()
    {
        if (!victoryMode)
        {
            UpdateSubmarine();
        }
    }

    // -------------------------------------------------------------------------
    // Public API — Gameplay
    // -------------------------------------------------------------------------

    // Cycle the submarine up to the next row, wrapping from top back to bottom
    public void CycleDown()
    {
        if (currentRowIndex <= 0)
        {
            currentRowIndex = rowYPositions.Length - 1;
        }
        else
        {
            currentRowIndex--;
        }

        targetY = rowYPositions[currentRowIndex];
    }

    // Advance the submarine one step forward along the X axis
    public void MoveForward()
    {
        targetX += moveStepX;
    }

    // Snap the submarine back to its starting position and reset all motion state
    public void ResetToCenter()
    {
        currentRowIndex = 1;
        currentX = startX;
        targetX = startX;
        currentY = rowYPositions[currentRowIndex];
        targetY = currentY;
        bobTime = 0f;

        transform.position = new Vector3(currentX, currentY, transform.position.z);
        transform.rotation = Quaternion.identity;
        transform.localScale = originalScale;
    }

    // -------------------------------------------------------------------------
    // Public API — Victory
    // -------------------------------------------------------------------------

    // Glide the submarine to screen center then complete one figure-eight cycle
    public IEnumerator PlayVictoryFlip(float screenCenterX)
    {
        victoryMode = true;

        float centerY = rowYPositions[1];
        float velX = 0f;
        float velY = 0f;
        float elapsed = 0f;

        while (elapsed < victorySlideDuration * 3f)
        {
            elapsed += Time.deltaTime;

            currentX = Mathf.SmoothDamp(currentX, screenCenterX, ref velX, victorySlideDuration);
            currentY = Mathf.SmoothDamp(currentY, centerY, ref velY, victorySlideDuration);

            transform.position = new Vector3(currentX, currentY, transform.position.z);

            if (Mathf.Abs(currentX - screenCenterX) < 1f &&
                Mathf.Abs(currentY - centerY) < 1f)
            {
                break;
            }

            yield return null;
        }

        currentX = screenCenterX;
        currentY = centerY;

        transform.position = new Vector3(currentX, currentY, transform.position.z);
        transform.rotation = Quaternion.identity;

        yield return FigureEightCycle(screenCenterX, centerY);
    }

    // Begin looping the figure-eight animation indefinitely
    public void StartVictoryLoop()
    {
        if (victoryCoroutine != null)
        {
            StopCoroutine(victoryCoroutine);
        }

        victoryCoroutine = StartCoroutine(VictoryLoopCoroutine(currentX, currentY));
    }

    // Stop the victory loop and restore normal movement state
    public void StopVictory()
    {
        if (victoryCoroutine != null)
        {
            StopCoroutine(victoryCoroutine);
            victoryCoroutine = null;
        }

        victoryMode = false;

        transform.rotation = Quaternion.identity;
        transform.localScale = originalScale;
    }

    // -------------------------------------------------------------------------
    // Internal Helpers
    // -------------------------------------------------------------------------

    // Snap position and tracking variables to the given row index
    void SnapToRow(int rowIndex)
    {
        currentY = rowYPositions[rowIndex];
        targetY = currentY;

        Vector3 pos = transform.position;
        pos.y = currentY;
        transform.position = pos;
    }

    // Lerp X and Y toward their targets and apply the bob offset
    void UpdateSubmarine()
    {
        currentY = Mathf.Lerp(currentY, targetY, Time.deltaTime * slideSpeed);
        currentX = Mathf.Lerp(currentX, targetX, Time.deltaTime * forwardSlideSpeed);

        bobTime += Time.deltaTime;

        float newY = currentY + Mathf.Sin(bobTime * bobSpeed * Mathf.PI * 2f) * bobAmplitude;

        transform.position = new Vector3(currentX, newY, transform.position.z);
    }

    // Repeat figure-eight cycles indefinitely until stopped
    IEnumerator VictoryLoopCoroutine(float cx, float cy)
    {
        while (true)
        {
            yield return FigureEightCycle(cx, cy);
        }
    }

    // Animate one full Lissajous figure-eight cycle around the given center point
    IEnumerator FigureEightCycle(float cx, float cy)
    {
        float elapsed = 0f;

        while (elapsed < loopCycleDuration)
        {
            elapsed += Time.deltaTime;

            float t = (elapsed / loopCycleDuration) * Mathf.PI * 2f;

            currentX = cx + loopRadiusX * Mathf.Sin(t);
            currentY = cy + loopRadiusY * Mathf.Sin(2f * t);

            transform.position = new Vector3(currentX, currentY, transform.position.z);

            yield return null;
        }

        currentX = cx;
        currentY = cy;

        transform.position = new Vector3(currentX, currentY, transform.position.z);
    }
}