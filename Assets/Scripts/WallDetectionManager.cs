using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class WallDetectionManager : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public Camera arCamera;
    private Vector3 floorPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Enable detection of both horizontal and vertical planes
        planeManager.requestedDetectionMode = PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle touch inputs
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Perform a raycast to detect objects in the scene
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Debug.Log("User selected wall area.");
                    // Perform further actions, like highlighting or modifying the wall area
                    HighlightWallArea(hit.collider.gameObject);
                }
            }
        }
    }

    // Highlights the selected wall area
    void HighlightWallArea(GameObject wall)
    {
        // Change the wall's material color to green to highlight it
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        if (wallRenderer != null)
        {
            wallRenderer.material.color = Color.green;
        }
    }

    // Called when a plane is added by ARPlaneManager
    public void OnPlaneAdded(ARPlane plane)
    {
        // Correctly identify horizontal planes (floors)
        if (plane.alignment == PlaneAlignment.HorizontalUp)
        {
            floorPosition = plane.transform.position;
            Debug.Log("Floor detected at position: " + floorPosition);
        }
    }

    // Called when a vertical plane (wall) is detected
    public void OnWallDetected(ARPlane plane)
    {
        if (plane.alignment == PlaneAlignment.Vertical)
        {
            Debug.Log("Wall detected!");
        }
    }
}
