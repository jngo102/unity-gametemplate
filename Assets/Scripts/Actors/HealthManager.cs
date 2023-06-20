using UnityEngine;

[RequireComponent(typeof(Facer))]
public class HealthManager : MonoBehaviour {
    [SerializeField] private float currentHealth = 5;
    public float CurrentHealth {
        get => currentHealth;
        private set {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    }

    [SerializeField] private float maxHealth = 5;
    public float MaxHealth {
        get => maxHealth;
        set {
            maxHealth = Mathf.Clamp(value, 0, MaxHealth);
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }
    }

    [SerializeField] private float invincibilityTime = 0.5f;
    public bool CanHurt { get; set; } = true;

    public delegate void OnHealthChange(float currentHealth, float maxHealth);
    public event OnHealthChange HealthChanged;

    public delegate void OnHarm(float damageAmount, Damager damageSource);
    public event OnHarm Harmed;
    
    private Facer facer;
    private float currentInvincibilityTime;

    private void Awake() {
        facer = GetComponent<Facer>();
        
        currentInvincibilityTime = invincibilityTime + 1;
    }

    private void Update() {
        if (currentInvincibilityTime < invincibilityTime) {
            currentInvincibilityTime = Mathf.Clamp(currentInvincibilityTime + Time.deltaTime, 0, invincibilityTime + 1);
        } else {
            CanHurt = true;
        }
    }

    public void Hurt(float damageAmount, Damager damageSource) {
        if (!CanHurt) return;
        if (damageSource != null) {
            facer.FaceObject(damageSource.transform);
        }
        CurrentHealth -= Mathf.Clamp(damageAmount, 0, CurrentHealth);
        Harmed?.Invoke(damageAmount, damageSource);
        CanHurt = false;
        currentInvincibilityTime = 0;
    }

    public void Heal(float healAmount) {
        CurrentHealth += Mathf.Clamp(healAmount, 0, MaxHealth - CurrentHealth);
    }

    public void FullHeal() {
        Heal(MaxHealth - CurrentHealth);
    }

    public void InstantKill() {
        Hurt(CurrentHealth, null);
    }
}
