using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

/// <summary>
/// Singleton that manages all the user interfaces in the game.    
/// </summary>
public class UIManager : Singleton<UIManager> {
    /// <summary>
    /// The fader object.
    /// </summary>
    [SerializeField] private Animator fader;

    /// <summary>
    /// The pause menu object.
    /// </summary>
    [SerializeField] private GameObject pauseMenu;

    /// <inheritdoc />
    protected override void OnAwake() {
        SetupPauseMenu();
    }

    /// <inheritdoc />
    private void OnEnable() {
        if (InputManager.Instance.Cancel == null) return;
        InputManager.Instance.Cancel.performed += TogglePauseMenu;
    }

    /// <inheritdoc />
    private void OnDisable() {
        if (InputManager.Instance == null || InputManager.Instance.Cancel == null) return;
        InputManager.Instance.Cancel.performed -= TogglePauseMenu;
    }

    /// <summary>
    /// Assign callbacks to pause menu buttons.
    /// </summary>
    private void SetupPauseMenu() {
        if (pauseMenu == null) {
            return;
        }

        foreach (var button in pauseMenu.GetComponentsInChildren<Button>(true)) {
            if (button.name.Contains("Resume")) {
                button.onClick.AddListener(() => ShowPauseMenu(false));
            } else if (button.name.Contains("Quit")) {
                button.onClick.AddListener(() => GameManager.Instance.ChangeScene("MainMenu"));
            }
        }
    }

    /// <summary>
    /// Toggle the pause menu; used in context of a performed input.
    /// </summary>
    private void TogglePauseMenu(InputAction.CallbackContext _) {
        if (!SceneData.IsGameplayScene(SceneManager.GetActiveScene().name) ||
            pauseMenu == null) return;

        ShowPauseMenu(!pauseMenu.activeSelf);
    }

    /// <summary>
    /// Show or hide the pause menu.
    /// </summary>
    /// <param name="show">Whether to show the pause menu.</param>
    public void ShowPauseMenu(bool show) {
        pauseMenu.SetActive(show);
        if (show) {
            GameManager.PauseGame();
        } else {
            GameManager.ResumeGame();
        }
    }

    /// <summary>
    /// Show the fader.
    /// </summary>
    public IEnumerator FadeIn() {
        if (fader == null) yield break;
        fader.gameObject.SetActive(true);
        fader.Play("Fade In");
        yield return new WaitUntil(() => fader.IsFinished());
    }

    /// <summary>
    /// Hide the fader.
    /// </summary>
    public IEnumerator FadeOut() {
        if (fader == null) yield break;
        fader.Play("Fade Out");
        yield return new WaitUntil(() => fader.IsFinished());
        fader.gameObject.SetActive(false);
    }
}
