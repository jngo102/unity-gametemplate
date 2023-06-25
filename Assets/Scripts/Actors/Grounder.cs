using UnityEngine;

/// <summary>
/// Manages whether an actor is grounded.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Grounder : MonoBehaviour {
    [SerializeField] private int numGroundCheckRays = 3;
    [SerializeField] private float groundCheckRayLength = 0.05f;

    private new Collider2D collider;

    /// <summary>
    /// Whether the actor was on the ground in the previous frame.
    /// </summary>
    public bool WasGrounded { get; private set; }

    /// <inheritdoc />
    private void Awake() {
        collider = GetComponent<Collider2D>();
    }

    /// <inheritdoc />
    private void Update() {
        WasGrounded = IsGrounded();
    }

    /// <summary>
    /// Check whether the actor is on the ground.
    /// </summary>
    /// <returns>Whether the actor is on the ground.</returns>
    public bool IsGrounded() {
        for (var rayIdx = 0; rayIdx < numGroundCheckRays; rayIdx++) {
            var rayOrigin =
                new Vector2(collider.bounds.min.x + collider.bounds.size.x * rayIdx / (numGroundCheckRays - 1),
                    collider.bounds.min.y);
#if UNITY_EDITOR
            Debug.DrawRay(rayOrigin, Vector2.down * groundCheckRayLength, Color.red);
#endif
            var hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckRayLength,
                LayerMask.GetMask("Terrain"));
            if (hit) {
                return true;
            }
        }

        return false;
    }
}
