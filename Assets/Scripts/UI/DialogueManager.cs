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

    private Dialogue currentDialogue;
    private int currentPage;

    /// <inheritdoc />
    public override void Open() {
        base.Open();
        if (InputManager.Instance == null) return;
        InputManager.Instance.Jump.InputAction.performed += OnSkip;
    }

    /// <inheritdoc />
    public override void Close() {
        if (InputManager.Instance == null) return;
        InputManager.Instance.Jump.InputAction.performed -= OnSkip;
        currentDialogue = null;
        base.Close();
    }

    /// <summary>
    /// Start a dialogue.
    /// </summary>
    /// <param name="dialogue">The dialogue data object.</param>
    public void StartDialogue(Dialogue dialogue) {
        foreach (var player in FindObjectsOfType<Player>(true)) {
            player.DisableAllInputs();
            player.StopMovement();
        }
        currentDialogue = dialogue;
        currentPage = 0;
        typer.Type(currentDialogue.Pages[currentPage]);
    }

    /// <summary>
    /// Advance the dialogue to the next page.
    /// </summary>
    private void NextPage() {
        currentPage++;
        if (currentPage >= currentDialogue.Pages.Length) {
            Close();
            foreach (var player in FindObjectsOfType<Player>(true)) {
                player.EnableAllInputs();
            }
            return;
        }
        
        typer.Type(currentDialogue.Pages[currentPage]);
    }

    /// <summary>
    /// Callback to skip the typewriting effect with dialogue.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnSkip(InputAction.CallbackContext context) {
        if (typer.IsPrinting) {
            typer.Skip();
        } else {
            NextPage();
        }
    }
}
