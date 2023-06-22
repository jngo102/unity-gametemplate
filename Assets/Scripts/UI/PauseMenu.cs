using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controller for the pause menu user interface.
/// </summary>
public class PauseMenu : BaseUI {
    [SerializeField] private VerticalLayoutGroup menuButtons;
    [SerializeField] private VerticalLayoutGroup quitConfirmation;

    /// <inheritdoc />   
    private void Awake() {
        InputManager.Instance.Cancel.performed += OnCancel;
    }

    /// <inheritdoc />
    private void OnDestroy() {
        InputManager.Instance.Cancel.performed -= OnCancel;
    }

    /// <summary>
    /// Callback to toggle the pause menu.
    /// </summary>
    /// <param name="context"></param>
    private void OnCancel(InputAction.CallbackContext context) {
        CloseQuitConfirmation();
        Toggle();
    }

    /// <inheritdoc />
    public override void Open() {
        base.Open();
        GameManager.PauseGame();
    }

    /// <inheritdoc />
    public override void Close() {
        base.Close();
        GameManager.ResumeGame();
    }

    /// <summary>
    /// Ask whether the player actually wants to quit the game.
    /// </summary>
    public void AskForQuitConfirmation() {
        menuButtons.gameObject.SetActive(false);
        quitConfirmation.gameObject.SetActive(true);
    }

    /// <summary>
    /// Close the quit confirmation popup.
    /// </summary>
    public void CloseQuitConfirmation() {
        quitConfirmation.gameObject.SetActive(false);
        menuButtons.gameObject.SetActive(true);
    }

    /// <summary>
    /// Quit to the main menu scene.
    /// </summary>
    public void QuitToMainMenu() {
        CloseQuitConfirmation();
        Close();
        GameManager.Instance.ChangeScene("MainMenu");
    }
}
