using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for the pause menu user interface.
/// </summary>
public class PauseMenu : BaseUI
{
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
    /// Quit to the main menu scene.
    /// </summary>
    public void QuitToMainMenu() {
        Close();
        GameManager.Instance.ChangeScene("MainMenu");
    }
}
