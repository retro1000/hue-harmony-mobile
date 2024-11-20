using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetectionController : MonoBehaviour
{
    private ARPlaneManager _planeManager;

    void Start()
    {
        // Get the ARPlaneManager component from the GameObject
        _planeManager = GetComponent<ARPlaneManager>();

        if (_planeManager != null)
        {
            // Enable plane detection
            _planeManager.enabled = true;

            // Set the detection mode to detect both horizontal and vertical planes
            _planeManager.detectionMode = PlaneDetectionMode.Vertical;

            // Subscribe to the planesChanged event to track when planes are added, updated, or removed
            _planeManager.planesChanged += OnPlanesChanged;
        }
        else
        {
            Debug.LogError("ARPlaneManager component not found on this GameObject.");
        }
    }

    // This method is called whenever planes are added, updated, or removed
    private void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
    {
        // Log planes that were added
        foreach (ARPlane addedPlane in eventArgs.added)
        {
            Debug.Log("Plane added: " + addedPlane.trackableId);
        }

        // Log planes that were updated
        foreach (ARPlane updatedPlane in eventArgs.updated)
        {
            Debug.Log("Plane updated: " + updatedPlane.trackableId);
        }

        // Log planes that were removed
        foreach (ARPlane removedPlane in eventArgs.removed)
        {
            Debug.Log("Plane removed: " + removedPlane.trackableId);
        }
    }

    // Unsubscribe from the planesChanged event when the object is destroyed
    void OnDestroy()
    {
        if (_planeManager != null)
        {
            _planeManager.planesChanged -= OnPlanesChanged;
        }
    }
}
