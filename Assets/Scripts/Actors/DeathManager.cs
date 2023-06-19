using UnityEngine;

[RequireComponent(typeof(Facer))]
[RequireComponent(typeof(HealthManager))]
public class DeathManager : MonoBehaviour
{
    [SerializeField] private Facer facer;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private GameObject corpsePrefab;

    private void Awake() {
        healthManager.Die += Die;
    }

    private void Die() {
        if (corpsePrefab != null) {
            Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
