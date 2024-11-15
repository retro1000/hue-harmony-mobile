using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SelectPoints : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject pointPrefab; // Prefab representing the selected point
    private List<Vector3> selectedPoints = new List<Vector3>(); // Store selected points
    private List<GameObject> pointObjects = new List<GameObject>(); // List of instantiated point objects

    void Update()
    {
        // Detect touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    
                    // Instantiate a point at the hit location
                    GameObject pointObject = Instantiate(pointPrefab, hitPose.position, Quaternion.identity);
                    pointObjects.Add(pointObject);
                    selectedPoints.Add(hitPose.position);

                    // If 3 or more points, try to create the shape
                    if (selectedPoints.Count >= 3)
                    {
                        CreateShape();
                    }
                }
            }
        }
    }

    void CreateShape()
    {
        // Create a mesh or shape from the points (e.g., using LineRenderer)
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = selectedPoints.Count + 1; // Add an extra point to close the shape
        lineRenderer.SetPositions(selectedPoints.ToArray());
        lineRenderer.SetPosition(selectedPoints.Count, selectedPoints[0]); // Close the shape

        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.loop = true; // Close the loop
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = Color.red;
    }
}

