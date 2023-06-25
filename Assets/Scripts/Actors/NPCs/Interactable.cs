using UnityEngine;

/// <summary>
/// An interactable actor.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
    /// <summary>
    /// Automatically interact when the player activates the trigger.
    /// </summary>
    [SerializeField] protected bool autoInteract;

    /// <summary>
    /// Whether the player can interact with this actor.
    /// </summary>
    public bool CanInteract { get; private set; }

    /// <inheritdoc />
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (autoInteract) {
                Interact();
            } else {
                CanInteract = true;
            }
        }
    }

    /// <inheritdoc />
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            CanInteract = false;
        }
    }

    /// <summary>
    /// Interact with this actor.
    /// </summary>
    public abstract void Interact();
}
