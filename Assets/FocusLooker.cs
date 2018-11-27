using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FocusLooker : MonoBehaviour {
    /// <summary>
    /// The zoomed in FOV
    /// </summary>
    private const float FovZoom = 40;
    /// <summary>
    /// How often a zoom can occur. Used to stop it constantly
    /// entering and leaving zoom mode
    /// </summary>
    private const float TriggerTime = 1f;
    /// <summary>
    /// How much mouse movement to un-zoom
    /// </summary>
    private const float ToleranceToUnZoom = 1f;
    
    private bool _canZoom;
    private float _initFov;
    private bool _isZoomed;
    private float _nextZoomTime;

    public Camera Camera;

    // Use this for initialization
    private void Start() {
        _initFov = Camera.fieldOfView;
    }

    private void Update() {
        // If enough time has passed we can zoom again
        if (Time.time > _nextZoomTime) {
            _canZoom = true;
        }
        
        // See if we've done enough mouse movement
        var yRot = Mathf.Abs(CrossPlatformInputManager.GetAxis("Mouse X"));
        var xRot = Mathf.Abs(CrossPlatformInputManager.GetAxis("Mouse Y"));
        if (_isZoomed && (xRot > ToleranceToUnZoom || yRot > ToleranceToUnZoom)) {
            ResetZoom();
        }
    }


    // Update is called once per frame
    private void LateUpdate() {
        // Reset FOV
        Camera.fieldOfView = _initFov;

        // Back out if we can't zoom
        if (!_canZoom) {
            return;
        }

        var forward = Camera.transform.forward;
        var layer = 1 << LayerMask.NameToLayer("Focus");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, 100f, layer)) {
            var dist = Vector3.Distance(transform.position, hit.transform.position);
            var focusObject = hit.transform.GetComponent<FocusObject>();
            
            if (dist < focusObject.RangeRequirement) {
                Debug.Log("Looking at " + hit.transform.gameObject.name);

                _isZoomed = true;
                 if (focusObject.ZoomInRightGood) {
                     // Look at the object. In reality we probably want to slowly shift
                     // the camera to look at the object
                    Camera.transform.LookAt(focusObject.transform);
                     
                     // TODO: Something here to zoom in/out more/less depending on distance
                     //var offset = Mathf.InverseLerp(0, focusObject.RangeRequirement, dist) * 20;
                     //var fov = FovZoom - offset;     
                     //Debug.Log(fov);

                     var fov = FovZoom;
                     
                     Camera.fieldOfView = fov;
                 } else {
                    Camera.fieldOfView = FovZoom;
                }
            } else {
                ResetZoom();
            }
        } else {
            ResetZoom();
        }
    }
    
    /// <summary>
    /// Resets the zoom stuff back to default and kicks off the timer for the next
    /// possible zoom
    /// </summary>
    private void ResetZoom() {
        _canZoom = false;
        _isZoomed = false;
        _nextZoomTime = Time.time + TriggerTime;
    }
}