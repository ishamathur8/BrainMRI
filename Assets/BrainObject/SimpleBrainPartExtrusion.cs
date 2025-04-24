using UnityEngine;
using System.Collections.Generic;

public class SimpleBrainPartExtrusion : MonoBehaviour
{
    [Tooltip("The scale factor for extrusion.")]
    public float extrusionScaleFactor = 1.5f; // Reduced scale for simplicity

    [Tooltip("The distance to extrude the part.")]
    public float extrusionDistance = 0.05f; // Reduced distance for simplicity

    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();
    private Transform lastInteractedPart = null;
    private bool isExtruded = false;

    void Start()
    {
        // Store initial positions and scales of all direct child transforms
        foreach (Transform child in transform)
        {
            originalPositions[child] = child.localPosition;
            originalScales[child] = child.localScale;
        }
    }

    public void ExtrudeOrRevert(Transform part)
    {
        if (part != null && originalPositions.ContainsKey(part))
        {
            if (part == lastInteractedPart && isExtruded)
            {
                // Revert to original
                part.localPosition = originalPositions[part];
                part.localScale = originalScales[part];
                isExtruded = false;
                lastInteractedPart = null;
            }
            else
            {
                // Extrude
                lastInteractedPart = part;
                Vector3 extrusionDirection = part.up; // Simple up direction for now
                part.localPosition = originalPositions[part] + extrusionDirection * extrusionDistance;
                part.localScale = originalScales[part] * extrusionScaleFactor;
                isExtruded = true;
            }
        }
    }
}