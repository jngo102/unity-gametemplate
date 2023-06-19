using UnityEngine;

[RequireComponent(typeof(Grounder))]
[RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour {
    [SerializeField] private float fallingGravityScale = 5;
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float risingGravityScale = 2;

    public delegate void JumpEvent(Jumper jumper);
    public event JumpEvent Jumped;
    public delegate void LandEvent(Jumper jumper);
    public event LandEvent Landed;

    private Rigidbody2D body;
    private Grounder grounder;

    public bool StopGravity { get; set; }

    private float jumpForce;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        grounder = GetComponent<Grounder>();

        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * risingGravityScale * body.mass));
    }

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

    public void CancelJump() {
        if (body.velocity.y <= 0) return;

        body.velocity = new Vector2(body.velocity.x, 0);
    }

    public void Jump() {
        if (grounder.IsGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            Jumped?.Invoke(this);
        }
    }

    public void Land() {
        Landed?.Invoke(this);
    }
}
