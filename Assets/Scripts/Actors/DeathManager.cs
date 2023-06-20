using UnityEngine;

[RequireComponent(typeof(Facer))]
[RequireComponent(typeof(HealthManager))]
public class DeathManager : MonoBehaviour {
    [SerializeField] private GameObject corpsePrefab;

    private Facer facer;
    private HealthManager healthManager;

    public delegate void OnDeath();
    public event OnDeath Died;

    public bool IsDead { get; private set; }

    private void Awake() {
        facer = GetComponent<Facer>();
        healthManager = GetComponent<HealthManager>();
        healthManager.Harmed += CheckDead;
    }

    private void CheckDead(float currentHealth, Damager damageSource) {
        if (currentHealth <= 0) {
            Died?.Invoke();
            Die(damageSource);
        }
    }

    private void Die(Damager damageSource) {
        IsDead = true;
        facer.FaceObject(damageSource.transform);
        if (corpsePrefab != null) {
            var corpse = Instantiate(corpsePrefab, transform.position, Quaternion.identity);
            corpse.transform.localScale =
                new Vector3(Mathf.Sign(transform.localScale.x) * corpse.transform.localScale.x,
                    corpse.transform.localScale.y, corpse.transform.localScale.z);
        }

        Destroy(gameObject);
    }

    public void Revive() {
        IsDead = false;
        healthManager.FullHeal();
    }
}
