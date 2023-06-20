using System.Collections;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class GameManager : Singleton<GameManager> {
    public delegate void OnSceneChange(string sceneName);
    public event OnSceneChange SceneChanged;

    protected override void OnAwake() {
    }

    public void ChangeScene(string sceneName) {
        UIManager.Instance.ShowPauseMenu(false);
        StartCoroutine(ChangeSceneRoutine(sceneName));
        SceneChanged?.Invoke(sceneName);
    }

    public IEnumerator ChangeSceneRoutine(string sceneName) {
        yield return StartCoroutine(UIManager.Instance.FadeIn());
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return StartCoroutine(UIManager.Instance.FadeOut());
    }

    public static void TogglePause() {
        if (Time.timeScale <= 0) {
            ResumeGame();
        } else {
            PauseGame();
        }
    }

    public static void PauseGame() => Time.timeScale = 0;

    public static void ResumeGame() => Time.timeScale = 1;

    public static void QuitGame() => Application.Quit();
}
