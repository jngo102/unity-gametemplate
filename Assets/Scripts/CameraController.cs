using UnityEngine;

public class CameraController : MonoBehaviour {
    public static Transform Target { get; set; }
    [SerializeField] private float smoothing = 5;

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
