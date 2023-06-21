using System.Collections;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

/// <summary>
/// Singleton that manages the game state.
/// </summary>
public class GameManager : Singleton<GameManager> {
    public delegate void OnSceneChange(string sceneName);
    public event OnSceneChange SceneChanged;

    /// <summary>
    /// Change scenes with a fade transition.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    public void ChangeScene(string sceneName) {
        UIManager.Instance.ShowPauseMenu(false);
        StartCoroutine(ChangeSceneRoutine(sceneName));
        SceneChanged?.Invoke(sceneName);
    }

    /// <summary>
    /// The routine that actually fades in, changes the scene, then fades out.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    /// <returns></returns>
    public IEnumerator ChangeSceneRoutine(string sceneName) {
        yield return StartCoroutine(UIManager.Instance.FadeIn());
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return StartCoroutine(UIManager.Instance.FadeOut());
    }

    /// <summary>
    /// Toggle whether the game is paused.
    /// </summary>
    public static void TogglePause() {
        if (Time.timeScale <= 0) {
            ResumeGame();
        } else {
            PauseGame();
        }
    }

    /// <summary>
    /// Pause the game.
    /// </summary>
    public static void PauseGame() => Time.timeScale = 0;

    /// <summary>
    /// Resume the game.
    /// </summary>
    public static void ResumeGame() => Time.timeScale = 1;

    /// <summary>
    /// Quit the game.
    /// </summary>
    public static void QuitGame() => Application.Quit();
}
