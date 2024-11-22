using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class WallPlaneCreator : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;

    // List to store user-selected points
    private List<Vector3> _selectedPoints = new List<Vector3>();

    // Prefab for visualizing selected points
    public GameObject pointPrefab;

    // Reference to the line renderer for connecting points
    private LineRenderer _lineRenderer;

    void Start()
    {
        // Get ARRaycastManager and ARPlaneManager components
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();

        if (_planeManager != null)
        {
            _planeManager.detectionMode = PlaneDetectionMode.Vertical;
        }

        // Add a LineRenderer component to the GameObject to visualize the lines
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;
        _lineRenderer.positionCount = 0;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.green;
        _lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        // Check for user input (touch on the screen)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Perform a raycast from the touch position
            Vector2 touchPosition = Input.GetTouch(0).position;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (_raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                // Get the first hit point on a vertical plane
                ARRaycastHit hit = hits[0];
                ARPlane plane = _planeManager.GetPlane(hit.trackableId);

                if (plane.alignment == PlaneAlignment.Vertical)
                {
                    // Add the hit point to the list of selected points
                    Vector3 selectedPoint = hit.pose.position;
                    _selectedPoints.Add(selectedPoint);

                    // Visualize the selected point
                    if (pointPrefab != null)
                    {
                        Instantiate(pointPrefab, selectedPoint, Quaternion.identity);
                    }

                    // Update the LineRenderer to connect the points
                    UpdateLineRenderer();
                }
            }
        }
    }

    // Updates the LineRenderer to connect the selected points
    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _selectedPoints.Count;

        for (int i = 0; i < _selectedPoints.Count; i++)
        {
            _lineRenderer.SetPosition(i, _selectedPoints[i]);
        }

        // Close the shape if enough points are selected
        if (_selectedPoints.Count > 2)
        {
            // Connect the last point to the first
            _lineRenderer.loop = true;

            // Optionally, generate a mesh from the selected points
            GenerateMesh();
        }
    }

    // Generate a mesh from the selected points
    private void GenerateMesh()
    {
        // Create a new GameObject for the mesh
        GameObject planeObject = new GameObject("GeneratedPlane");
        MeshFilter meshFilter = planeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = planeObject.AddComponent<MeshRenderer>();

        // Assign a material to the mesh
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Convert selected points to local coordinates relative to the plane object
        Vector3[] vertices = _selectedPoints.ToArray();

        // Generate triangles (assuming a convex polygon)
        int[] triangles = new int[(vertices.Length - 2) * 3];
        for (int i = 1; i < vertices.Length - 1; i++)
        {
            int triangleIndex = (i - 1) * 3;
            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i;
            triangles[triangleIndex + 2] = i + 1;
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Reset the selected points for the next shape
        _selectedPoints.Clear();
        _lineRenderer.positionCount = 0;
    }
}
