using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private Animator fader;
    [SerializeField] private GameObject pauseMenu;

    protected override void OnAwake() {
        if (pauseMenu != null) {
            foreach (var button in pauseMenu.GetComponentsInChildren<Button>(true)) {
                if (button.name.Contains("Resume")) {
                    button.onClick.AddListener(() => ShowPauseMenu(false));
                } else if (button.name.Contains("Quit")) {
                    button.onClick.AddListener(() => GameManager.Instance.ChangeScene("MainMenu"));
                }
            }
        }
    }
    private void OnEnable() {
        if (InputManager.Instance.Cancel == null) return;
        InputManager.Instance.Cancel.performed += TogglePauseMenu;
    }

    private void OnDisable() {
        if (InputManager.Instance.Cancel == null) return;
        InputManager.Instance.Cancel.performed -= TogglePauseMenu;
    }

    private void TogglePauseMenu(InputAction.CallbackContext _) {
        if (!SceneData.IsGameplayScene(SceneManager.GetActiveScene().name) ||
            pauseMenu == null) return;
        
        ShowPauseMenu(!pauseMenu.activeSelf);
    }

    public void ShowPauseMenu(bool show) {
        pauseMenu.SetActive(show);
        if (show) {
            GameManager.PauseGame();
        } else {
            GameManager.ResumeGame();
        }
    }

    public IEnumerator FadeIn() {
        if (fader == null) yield break;
        fader.gameObject.SetActive(true);
        fader.Play("Fade In");
        yield return new WaitUntil(() => fader.IsFinished());
    }

    public IEnumerator FadeOut() {
        if (fader == null) yield break;
        fader.Play("Fade Out");
        yield return new WaitUntil(() => fader.IsFinished());
        fader.gameObject.SetActive(false);
    }
}
