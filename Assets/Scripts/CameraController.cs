using UnityEngine;

/// <summary>
///     A camera that follows a target.
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Shaker))]
public class CameraController : MonoBehaviour {
    [SerializeField] private float smoothing = 5;

    private Shaker shaker;
    private Transform target;

    public Transform Target {
        get => target;
        set {
            target = value;
            ResetPosition();
        }
    }

    private void Awake() {
        shaker = GetComponent<Shaker>();
        GameManager.Instance.LevelStarted += ResetPosition;
    }

    private void Update() {
        FollowTarget();
    }

    /// <summary>
    /// Reset the camera to the target's position.
    /// </summary>
    private void ResetPosition() {
        if (!target) return;
        var targetPos = target.transform.position;
        var selfTransform = transform;
        selfTransform.position = new Vector3(targetPos.x, targetPos.y, selfTransform.position.z);
    }

    /// <summary>
    ///     Follow the specified target.
    /// </summary>
    private void FollowTarget() {
        if (!Target) return;
        var targetPos = Target.transform.position;
        var position = transform.position;
        position = Vector3.Lerp(position, new Vector3(targetPos.x, targetPos.y, position.z),
            smoothing * Time.deltaTime);
        transform.position = position;
    }
}