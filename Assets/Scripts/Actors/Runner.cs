using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {
    [SerializeField] private float runSpeed = 5;

    private Rigidbody2D body;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    public void Run(float direction) {
        var velocityX = direction * runSpeed;
        body.velocity = new Vector2(velocityX, body.velocity.y);
        var scaleX = transform.localScale.x;
        if (scaleX < 0 && velocityX > 0 || scaleX > 0 && velocityX < 0) {
            Turn();
        }
    }

    private void Turn() {
        var localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
