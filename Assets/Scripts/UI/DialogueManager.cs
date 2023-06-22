using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages dialogue from NPCs.
/// </summary>
public class DialogueManager : BaseUI {
    /// <summary>
    /// The displayed dialogue typewriter.
    /// </summary>
    [SerializeField] private Typewriter typer;

    /// <inheritdoc />
    public override void Open() {
        if (InputManager.Instance == null) return;
        InputManager.Instance.Jump.InputAction.performed += OnSkip;
    }

    /// <inheritdoc />
    public override void Close() {
        if (InputManager.Instance == null) return;
        InputManager.Instance.Jump.InputAction.performed -= OnSkip;
    }

    /// <summary>
    /// Callback to skip the typewriting effect with dialogue.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnSkip(InputAction.CallbackContext context) {
        typer.Skip();
    }
}
