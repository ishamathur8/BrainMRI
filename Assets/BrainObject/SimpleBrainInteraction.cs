using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems; // If you are using the new Input System

public class SimpleBrainInteraction : MonoBehaviour
{
    private Camera arCamera;
    public SimpleBrainPartExtrusion extrusionScript;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    void Start()
    {
        // Find the ARCamera
        ARSessionOrigin arOrigin = FindObjectOfType<ARSessionOrigin>();
        if (arOrigin != null && arOrigin.camera != null)
        {
            arCamera = arOrigin.camera;
        }
        else
        {
            Debug.LogError("ARSessionOrigin or ARCamera not found. Make sure an ARSessionOrigin exists in the scene.");
            enabled = false;
            return;
        }

        // Get the SimpleBrainPartExtrusion script
        extrusionScript = GetComponent<SimpleBrainPartExtrusion>();
        if (extrusionScript == null)
        {
            Debug.LogError("SimpleBrainPartExtrusion script not found on the same GameObject.");
            enabled = false;
            return;
        }

        // Get the ARRaycastManager
        raycastManager = FindObjectOfType<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager not found in the scene. Make sure to add it to the AR Session Origin.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);

                // Perform a raycast using ARRaycastManager against tracked objects
                if (raycastManager.Raycast(ray, hitResults, TrackableType.All))
                {
                    // Raycast hit something
                    if (hitResults.Count > 0)
                    {
                        foreach (var hit in hitResults)
                        {
                            ARTrackable arTrackable = hit.trackable;
                            if (arTrackable != null)
                            {
                                Transform hitTransform = arTrackable.transform;
                                // Check if the hit object is a direct child of this GameObject (the brain)
                                if (hitTransform.parent == transform)
                                {
                                    extrusionScript.ExtrudeOrRevert(hitTransform);
                                    // We've interacted with a brain part, so we can stop checking other hits for this touch
                                    break;
                                }
                            }
                        }
                        // Clear the hit results for the next frame
                        hitResults.Clear();
                    }
                }
            }
        }
    }
}