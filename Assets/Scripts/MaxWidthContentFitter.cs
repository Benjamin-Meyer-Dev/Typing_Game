// =============================================================================
// MaxWidthContentFitter.cs
// Mirrors Unity's ContentSizeFitter for horizontal fit but clamps the result
// to a maximum width. Attach this instead of ContentSizeFitter.
// =============================================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class MaxWidthContentFitter : UIBehaviour, ILayoutSelfController
{
    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Tooltip("The widest the panel is allowed to grow before it stops expanding.")]
    public float maxWidth = 1200f;

    [Tooltip("The narrowest the panel is allowed to shrink.")]
    public float minWidth = 0f;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private RectTransform rectTransform;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Cache the RectTransform reference
    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
    }

    // -------------------------------------------------------------------------
    // ILayoutSelfController — called by Unity's layout system each rebuild
    // -------------------------------------------------------------------------

    // Apply the clamped width when the layout system rebuilds horizontally
    public void SetLayoutHorizontal()
    {
        ApplyWidth();
    }

    // Vertical sizing is not managed by this component
    public void SetLayoutVertical()
    {
    }

    // -------------------------------------------------------------------------
    // Width Logic
    // -------------------------------------------------------------------------

    // Read the preferred width that child layout elements report, clamp it, and write it back onto our own RectTransform
    void ApplyWidth()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (rectTransform == null)
        {
            return;
        }

        float preferred = LayoutUtility.GetPreferredWidth(rectTransform);
        float clamped = Mathf.Clamp(preferred, minWidth, maxWidth);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clamped);
    }

#if UNITY_EDITOR
    // Trigger a layout rebuild when values are changed in the Inspector
    protected override void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this != null)
            {
                LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
            }
        };
    }
#endif
}