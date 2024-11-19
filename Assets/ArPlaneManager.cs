using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneInteraction : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject pointPrefab; // Prefab for visualizing points
    [SerializeField] private Transform pointParent;

    private List<Vector3> selectedPoints = new List<Vector3>();
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Raycast to detect a point on a vertical plane
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    // Instantiate a point visualizer
                    Instantiate(pointPrefab, hitPose.position, Quaternion.identity, pointParent);

                    // Store the selected point
                    selectedPoints.Add(hitPose.position);

                    // Draw the shape
                    DrawShape();
                }
            }
        }
    }

    void DrawShape()
    {
        if (selectedPoints.Count > 1)
        {
            lineRenderer.positionCount = selectedPoints.Count;
            lineRenderer.SetPositions(selectedPoints.ToArray());

            // Close the shape if the last point is near the first
            if (selectedPoints.Count > 2 && Vector3.Distance(selectedPoints[0], selectedPoints[^1]) < 0.05f)
            {
                lineRenderer.loop = true;
            }
        }
    }
}
