using UnityEngine;

/// <summary>
/// Manages horizontal movement for the actor.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Facer))]
public class Runner : MonoBehaviour {
    /// <summary>
    /// The speed at which the actor runs.
    /// </summary>
    [SerializeField] private float runSpeed = 5;

    private Rigidbody2D body;
    private Facer facer;

    /// <inheritdoc />
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        facer = GetComponent<Facer>();
    }

    /// <summary>
    /// Perform horizontal movement.
    /// </summary>
    /// <param name="direction">The direction that the actor runs in; negative means to the left, positive to the right.</param>
    public void Run(float direction) {
        var velocityX = direction * runSpeed;
        body.velocity = new Vector2(velocityX, body.velocity.y);
        var scaleX = transform.localScale.x;
        if (scaleX < 0 && velocityX > 0 || scaleX > 0 && velocityX < 0) {
            facer.Flip();
        }
    }
}
