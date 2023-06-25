using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages dialogue from NPCs.
/// </summary>
public class DialogueManager : BaseUI {
    /// <summary>
    [SerializeField] private Typewriter typer;
    /// The displayed dialogue typewriter.
    /// </summary>

    /// <summary>
    /// The text object containing the current dialogue.
    /// </summary>
    [SerializeField] private TextMeshProUGUI text;

    private Dialogue currentDialogue;
    private int currentPage;

    /// <summary>
    /// The currently displayed, translated dialogue text.
    /// </summary>
    private string CurrentText => currentDialogue.Pages[currentPage].GetLocalizedString();

    /// <inheritdoc />
    public override void Open() {
        base.Open();

        foreach (var playerInputManager in FindObjectsOfType<PlayerInputManager>(true)) {
            playerInputManager.Disable();
        }

        if (UIManager.Instance) {
            UIManager.Instance.Actions.Submit.performed += OnSubmit;
        }
    }

    /// <inheritdoc />
    public override void Close() {
        currentDialogue = null;

        if (UIManager.Instance) {
            UIManager.Instance.Actions.Submit.performed -= OnSubmit;
        }

        foreach (var playerInputManager in FindObjectsOfType<PlayerInputManager>(true)) {
            playerInputManager.Enable();
        }

        base.Close();
    }

    /// <summary>
    /// Start a dialogue.
    /// </summary>
    /// <param name="dialogue">The dialogue data object.</param>
    public void StartDialogue(Dialogue dialogue) {
        currentDialogue = dialogue;
        currentPage = 0;
        typer.Type(CurrentText);
    }

    /// <summary>
    /// Advance the dialogue to the next page.
    /// </summary>
    private void NextPage() {
        currentPage++;
        if (currentPage >= currentDialogue.Pages.Length) {
            Close();
            return;
        }

        typer.Type(CurrentText);
    }

    /// <summary>
    /// Callback to skip the typewriting effect with dialogue.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnSubmit(InputAction.CallbackContext context) {
        if (!context.ReadValueAsButton()) return;
        if (typer.IsPrinting) {
            typer.Skip();
        } else {
            NextPage();
        }
    }
}
