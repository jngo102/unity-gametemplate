using UnityEngine;

/// <summary>
/// Handles jumping for an actor.
/// </summary>
[RequireComponent(typeof(Grounder))]
[RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour {
    /// <summary>
    /// The gravity scale of the actor's rigid body when falling.
    /// </summary>
    [SerializeField] private float fallingGravityScale = 5;
    
    /// <summary>
    /// The maximum height that the actor may jump.
    /// </summary>
    [SerializeField] private float jumpHeight = 2;

    /// <summary>
    /// The gravity scale of the actor's rigid body when rising.
    /// </summary>
    [SerializeField] private float risingGravityScale = 2;

    public delegate void JumpEvent();

    /// <summary>
    /// Raised when the actor jumps.
    /// </summary>
    public event JumpEvent Jumped;
    
    public delegate void LandEvent();

    /// <summary>
    /// Raised when the actor lands.
    /// </summary>
    public event LandEvent Landed;

    private Rigidbody2D body;
    private Grounder grounder;

    /// <summary>
    /// Whether to disable gravity for the actor.
    /// </summary>
    public bool StopGravity { get; set; }

    /// <summary>
    /// The force to apply to the actor when jumping.
    /// </summary>
    private float jumpForce;

    /// <inheritdoc />
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        grounder = GetComponent<Grounder>();

        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * risingGravityScale * body.mass));
    }

    /// <inheritdoc />
    private void Update() {
        if (body.velocity.y <= 0 && !grounder.WasGrounded && grounder.IsGrounded()) {
            Land();
        }

        body.gravityScale = body.velocity.y switch {
            > 0 => risingGravityScale,
            <= 0 => fallingGravityScale,
            _ => body.gravityScale
        };

        if (StopGravity) body.gravityScale = 0;
    }

    /// <summary>
    /// Cancel a jump.
    /// </summary>
    public void CancelJump() {
        if (body.velocity.y <= 0) return;

        body.velocity = new Vector2(body.velocity.x, 0);
    }

    /// <summary>
    /// Perform a jump.
    /// </summary>
    public void Jump() {
        if (grounder.IsGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            Jumped?.Invoke();
        }
    }
    
    /// <summary>
    /// Perform a landing.
    /// </summary>
    public void Land() {
        Landed?.Invoke();
    }
}
