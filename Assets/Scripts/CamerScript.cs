using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class WallSelector : MonoBehaviour
{
    public Camera arCamera;
    public ARRaycastManager raycastManager;
    public Material highlightMaterial; // Material to highlight the wall
    public Material selectedMaterial; // Material for coloring the wall

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // Perform a raycast from the touch position
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // Get the ARTrackable (e.g., ARPlane) associated with the hit
                    ARPlane plane = hits[0].trackable as ARPlane;

                    if (plane != null && plane.gameObject.CompareTag("Wall")) // Ensure it's a wall
                    {
                        // Apply the selected material to the wall
                        MeshRenderer meshRenderer = plane.gameObject.GetComponent<MeshRenderer>();

                        if (meshRenderer != null)
                        {
                            meshRenderer.material = selectedMaterial; // Change wall color
                        }
                    }
                }
            }
        }
    }
}
