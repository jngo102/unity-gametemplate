using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : Interactable {
    /// <summary>
    /// The dialogue object used when interacting with this NPC.
    /// </summary>
    [SerializeField] private Dialogue dialogue;

    /// <inheritdoc />
    private void OnEnable() {
        InputManager.Instance.Move.performed += OnInteract;
    }

    /// <inheritdoc />
    private void OnDisable() {
        InputManager.Instance.Move.performed -= OnInteract;
    }

    /// <summary>
    /// Callback for when the player manually interacts with this NPC.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnInteract(InputAction.CallbackContext context) {
        if (CanInteract && !autoInteract && Mathf.Abs(context.ReadValue<Vector2>().y) > 0) {
            Interact();
        }
    }

    /// <inheritdoc />
    public override void Interact() {
        var dialogueManager = UIManager.Instance.OpenUI<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);
    }
}
