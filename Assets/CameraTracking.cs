using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour {

    [SerializeField]
    private Transform[] follow;

    [SerializeField]
    private bool useFov;

    [SerializeField]
    private float minDistance, maxDistance;

    [SerializeField]
    private float minFov, maxFov;

    [SerializeField]
    private float sizeAdder, sizeMultiplier;

    private Camera _camera;

    private void Awake() {
        _camera = GetComponentInChildren<Camera>();
    }

    private void LateUpdate() {
        Bounds bounds = GetBounds();

        transform.position = transform.TransformPoint(bounds.center);

        Vector2 size = (Vector2)bounds.size + Vector2.one * sizeAdder * 2;
        float frustumHeight = Mathf.Max(size.x / _camera.aspect, size.y) * sizeMultiplier;

        if (_camera.orthographic) {
            _camera.orthographicSize = frustumHeight * .5f;
        } else if (useFov) {
            float fov = 2f * Mathf.Atan(frustumHeight * .5f / -_camera.transform.localPosition.z) * Mathf.Rad2Deg;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            _camera.fieldOfView = fov;
        } else {
            float distance = frustumHeight * .5f / Mathf.Tan(_camera.fieldOfView * .5f * Mathf.Deg2Rad);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            _camera.transform.localPosition = new Vector3(0, 0, -distance);
        }
    }

    private Bounds GetBounds() {
        Bounds bounds = new Bounds();
        if (_camera.orthographic) {
            foreach (Transform t in follow) {
                Vector3 cameraOriginPosition = transform.InverseTransformPoint(t.position);
                bounds.Encapsulate(cameraOriginPosition);
            }
        } else {
            foreach (Transform t in follow) {
                Vector3 viewportPosition = _camera.WorldToViewportPoint(t.position);
                float viewportDistance = viewportPosition.z;
                viewportPosition.z = -_camera.transform.localPosition.z;
                Vector3 worldPosition = _camera.ViewportToWorldPoint(viewportPosition);
                //debugPoints.Add(worldPosition);
                Vector3 cameraOriginPosition = transform.InverseTransformPoint(worldPosition);
                if (viewportDistance >= -_camera.transform.localPosition.z) {
                    bounds.Encapsulate(cameraOriginPosition);
                }
            }
        }
        return bounds;
    }

    //private List<Vector3> debugPoints = new List<Vector3>();
    //private void OnDrawGizmos() {
    //    while (debugPoints.Count > 0) {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(debugPoints[0], 1);
    //        debugPoints.RemoveAt(0);
    //    }
    //}

}