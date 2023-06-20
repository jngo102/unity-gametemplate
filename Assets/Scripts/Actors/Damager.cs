using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour {
    [SerializeField] private float damageAmount = 1;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent<HealthManager>(out var healthManager)) {
            healthManager.Hurt(damageAmount, this);
        }
    }
}
