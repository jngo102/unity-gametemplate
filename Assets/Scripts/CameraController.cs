using UnityEngine;

/// <summary>
/// A camera that follows a target.
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Shaker))]
public class CameraController : MonoBehaviour {
    private Transform target;
    public Transform Target {
        get => target;
        set {
            target = value;
            if (target != null) {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            }
        }
    }
    [SerializeField] private float smoothing = 5;

    private Shaker shaker;

    private void Awake() {
        shaker = GetComponent<Shaker>();
    }

    private void Update() {
        FollowTarget();
    }

    /// <summary>
    /// Follow the specified target.
    /// </summary>
    private void FollowTarget() {
        if (Target == null) return;
        var targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
