using UnityEngine;

public class Facer : MonoBehaviour {
    public void FaceObject(Transform target) {
        if (target.position.x > transform.position.x && transform.localScale.x < 0 ||
            target.position.x < transform.position.x && transform.localScale.x > 0) {
            Flip();
        }
    }

    public void Flip() {
        var localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
