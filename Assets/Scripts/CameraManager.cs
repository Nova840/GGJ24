using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    private float moveSmoothingSpeed, zoomSmoothingSpeed;

    [SerializeField]
    private AnimationCurve cameraDistanceAtBoundsSize;

    [SerializeField]
    private float playerTrackingHeight;

    private Camera _camera;

    private List<Player> players = new List<Player>();
    public void AddPlayerTransform(Player player) => players.Add(player);

    private void Awake() {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Start() {
        if (players.Count == 0) return;
        Bounds bounds = GetRelativeBounds();
        transform.position = GetTargetPosition(bounds);
        _camera.transform.localPosition = new Vector3(0, 0, -GetTargetDistance(bounds));
    }

    private void LateUpdate() {
        if (players.Count == 0) return;
        Bounds bounds = GetRelativeBounds();
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(bounds), moveSmoothingSpeed * Time.deltaTime);
        _camera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(_camera.transform.localPosition.z, -GetTargetDistance(bounds), zoomSmoothingSpeed * Time.deltaTime));
    }

    private Bounds GetRelativeBounds() {
        Vector3 GetPlayerRelativePosition(Player player) {
            Vector3 position = player.transform.position;
            position.y = playerTrackingHeight;
            return transform.InverseTransformPoint(position);
        }

        List<Vector3> relativePositions = new List<Vector3>();

        for (int i = 0; i < players.Count; i++) {
            if (players[i].PlayerMove.Respawning) continue;
            relativePositions.Add(GetPlayerRelativePosition(players[i]));
        }

        if (relativePositions.Count == 0) {
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        Bounds bounds = new Bounds(relativePositions[0], Vector3.zero);
        for (int i = 1; i < relativePositions.Count; i++) {
            bounds.Encapsulate(relativePositions[i]);
        }
        return bounds;
    }

    private Vector3 GetTargetPosition(Bounds bounds) {
        Vector3 center = transform.TransformPoint(bounds.center);
        return center + transform.rotation * Vector3.back * bounds.extents.z;
    }

    private float GetTargetDistance(Bounds bounds) {
        float size = Mathf.Max(bounds.size.x, bounds.size.y);
        return cameraDistanceAtBoundsSize.Evaluate(size);
    }

}