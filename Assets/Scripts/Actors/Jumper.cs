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

    /// <summary>
    /// The rate at which the actor falls during a jump.
    /// </summary>
    private float FallingAcceleration => Physics2D.gravity.y * body.mass * fallingGravityScale;

    /// <summary>
    /// The rate at which the actor rises during a jump.
    /// </summary>
    private float RisingAcceleration => Physics2D.gravity.y * body.mass * risingGravityScale;

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

        jumpForce = Mathf.Sqrt(-2 * jumpHeight * RisingAcceleration);
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
    /// Perform a vertical jump.
    /// </summary>
    public void Jump() {
        if (grounder.IsGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            Jumped?.Invoke();
        }
    }
    
    /// <summary>
    /// Jump with a specific force.
    /// </summary>
    /// <param name="force">The force to jump with.</param>
    public void JumpWith(Vector2 force) {
        var originalJumpForce = jumpForce;
        jumpForce = force.y;
        body.velocity = new Vector2(force.x, 0);
        Jump();
        jumpForce = originalJumpForce;
    }

    /// <summary>
    /// Jump vertically for a certain duration.
    /// </summary>
    /// <param name="time">The length of time to jump for.</param>
    public void JumpFor(float time) {
        var originalJumpForce = jumpForce;
        var riseTime = Mathf.Sqrt(-2 / RisingAcceleration);
        var fallTime = time - riseTime;
        Debug.Log($"Rise time: {riseTime}\tfall time: {fallTime}");
        jumpForce = -0.5f * (riseTime * RisingAcceleration + fallTime * FallingAcceleration);
        Debug.Log($"Jump force {jumpForce} for time {time}");
        Jump();
        jumpForce = originalJumpForce;
    }

    /// <summary>
    /// Jump to a specific position.
    /// </summary>
    /// <param name="targetPosition">The position to jump to.</param>
    /// <param name="jumpTime">The duration of the jump.</param>
    /// <param name="jumpHeight">The height of the jump at its apex.</param>
    public void JumpTo(Vector3 targetPosition, float jumpTime, float jumpHeight) {
        var originalJumpForce = jumpForce;
        var diff = targetPosition - transform.position;
        var jumpX = diff.x / jumpTime;
        var riseTime = Mathf.Sqrt(-2 * jumpHeight / RisingAcceleration);
        var fallTime = jumpTime - riseTime;
        Debug.Log($"Rise time: {riseTime}\tfall time: {fallTime}");
        jumpForce = Mathf.Sqrt(-2 * jumpHeight * (RisingAcceleration * riseTime + FallingAcceleration * fallTime));
        Debug.Log("Jump force: " + jumpForce);
        body.velocity = new Vector2(jumpX, 0);
        Jump();
        jumpForce = originalJumpForce;
    }

    /// <summary>
    /// Perform a landing.
    /// </summary>
    public void Land() {
        Landed?.Invoke();
    }
}
