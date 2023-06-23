using System.Collections;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

/// <summary>
/// Singleton that manages the game state.
/// </summary>
public class GameManager : Singleton<GameManager> {
    /// <summary>
    /// Change scenes with a fade transition.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    public void ChangeScene(string sceneName) {
        StartCoroutine(ChangeSceneRoutine(sceneName));
    }

    /// <summary>
    /// The routine that actually fades in, changes the scene, then fades out.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    /// <returns></returns>
    public IEnumerator ChangeSceneRoutine(string sceneName) {
        var fader = UIManager.Instance.GetUI<Fader>();
        yield return fader.FadeIn();
        SaveDataManager.Instance.SaveGame();
        yield return SceneManager.LoadSceneAsync(sceneName);
        SaveDataManager.Instance.LoadGame();
        yield return fader.FadeOut();
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
