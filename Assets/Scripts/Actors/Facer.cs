using UnityEngine;

/// <summary>
/// Deals with facing an actor towards a target and flipping their scale.
/// </summary>
public class Facer : MonoBehaviour {
    /// <summary>
    /// Face a target object.
    /// </summary>
    /// <param name="target">The target to face.</param>
    public void FaceObject(Transform target) {
        if (target.position.x > transform.position.x && transform.localScale.x < 0 ||
            target.position.x < transform.position.x && transform.localScale.x > 0) {
            Flip();
        }
    }

    /// <summary>
    /// Flip the actor's horizontal scale.
    /// </summary>
    public void Flip() {
        var selfTransform = transform;
        var localScale = selfTransform.localScale;
        localScale.x *= -1;
        selfTransform.localScale = localScale;
    }
}
