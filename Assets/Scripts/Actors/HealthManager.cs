using UnityEngine;

/// <summary>
///     Handles the actor's state of health.
/// </summary>
[RequireComponent(typeof(Facer))]
public class HealthManager : MonoBehaviour {
    public delegate void OnHarm(float damageAmount, Damager damageSource);

    public delegate void OnHealthChange(float currentHealth, float maxHealth);

    /// <summary>
    ///     The actor's current health.
    /// </summary>
    [SerializeField] private float currentHealth = 5;

    /// <summary>
    ///     The maximum amount of health that the actor can have.
    /// </summary>
    [SerializeField] private float maxHealth = 5;

    /// <summary>
    ///     The amount of time that the actor is invincible for after taking damage.
    /// </summary>
    [SerializeField] private float invincibilityTime = 0.5f;

    private float currentInvincibilityTime;

    private Facer facer;

    /// <summary>
    ///     The actor's current health.
    /// </summary>
    public float CurrentHealth {
        get => currentHealth;
        set {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    }

    /// <summary>
    ///     The maximum amount of health that the actor can have.
    /// </summary>
    public float MaxHealth {
        get => maxHealth;
        set {
            maxHealth = Mathf.Clamp(value, 0, MaxHealth);
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }
    }

    /// <summary>
    ///     Whether the actor can be hurt.
    /// </summary>
    public bool CanHurt { get; set; } = true;

    private void Awake() {
        facer = GetComponent<Facer>();

        currentInvincibilityTime = invincibilityTime + 1;
    }

    private void Update() {
        if (currentInvincibilityTime < invincibilityTime)
            currentInvincibilityTime = Mathf.Clamp(currentInvincibilityTime + Time.deltaTime, 0, invincibilityTime + 1);
        else
            CanHurt = true;
    }

    /// <summary>
    ///     Raised when the actor's health changes.
    /// </summary>
    public event OnHealthChange HealthChanged;

    /// <summary>
    ///     Raised when the actor takes damage.
    /// </summary>
    public event OnHarm Harmed;

    /// <summary>
    ///     Damage and take health away from the actor.
    /// </summary>
    /// <param name="damageAmount">The amount of damage inflicted.</param>
    /// <param name="damageSource">The source of the damage.</param>
    public void Hurt(float damageAmount, Damager damageSource) {
        if (!CanHurt) return;
        if (damageSource != null) facer.FaceObject(damageSource.transform);
        CurrentHealth -= Mathf.Clamp(damageAmount, 0, CurrentHealth);
        Harmed?.Invoke(damageAmount, damageSource);
        CanHurt = false;
        currentInvincibilityTime = 0;
    }

    /// <summary>
    ///     Heal the actor.
    /// </summary>
    /// <param name="healAmount">The amount of health to heal for.</param>
    public void Heal(float healAmount) {
        CurrentHealth += Mathf.Clamp(healAmount, 0, MaxHealth - CurrentHealth);
    }

    /// <summary>
    ///     Fully heal the actor.
    /// </summary>
    public void FullHeal() {
        Heal(MaxHealth - CurrentHealth);
    }

    /// <summary>
    ///     Instantly take away all of the actor's health.
    /// </summary>
    public void InstantKill() {
        Hurt(CurrentHealth, null);
    }
}