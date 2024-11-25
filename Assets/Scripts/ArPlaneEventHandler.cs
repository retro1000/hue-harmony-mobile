using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneEventHandler : MonoBehaviour
{
    public WallDetectionManager wallDetectionManager;

    // Subscribe to the plane added event
    void OnEnable()
    {
        ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
        planeManager.planesChanged -= OnPlanesChanged;
    }

    // Handle when planes are added, updated, or removed
    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane addedPlane in args.added)
        {
            if (addedPlane.alignment == PlaneAlignment.HorizontalUpward)
            {
                wallDetectionManager.OnPlaneAdded(addedPlane);
            }
            else if (addedPlane.alignment == PlaneAlignment.Vertical)
            {
                wallDetectionManager.OnWallDetected(addedPlane);
            }
        }
    }
}
