using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

/// <summary>
/// Singleton that manages all the user interfaces in the game.    
/// </summary>
public class UIManager : Singleton<UIManager> {
    /// <summary>
    /// The canvas to parent user interfaces to.
    /// </summary>
    [SerializeField] private Canvas canvas;

    /// <summary>
    /// All the user interface that are managed by the ui manager.
    /// </summary>
    [SerializeField] private BaseUI[] uis;

    /// <inheritdoc />
    protected override void OnAwake() {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    /// <summary>
    /// Callback for when a scene changes.
    /// </summary>
    /// <param name="prevScene">The previous scene that was changed from.</param>
    /// <param name="nextScene">The next scene that has been changed to.</param>
    private void OnSceneChange(Scene prevScene, Scene nextScene) {
        if (SceneData.IsGameplayScene(nextScene.name)) {
            CloseUI<PauseMenu>();
        } else {
            DestroyUI<PauseMenu>();
        }
    }

    /// <summary>
    /// Open a user interface.
    /// </summary>
    /// <typeparam name="T">The type of user interface to open.</typeparam>
    /// <returns>The user interface that was opened.</returns>
    public T OpenUI<T>() where T : BaseUI {
        var ui = GetUI<T>();
        if (ui == null) return null;
        ui.Open();
        return ui;
    }

    /// <summary>
    /// Close a user interface.
    /// </summary>
    /// <typeparam name="T">The type of user interface to close.</typeparam>
    /// <returns>Whether the user interace was successfully closed.</returns>
    public bool CloseUI<T>() where T : BaseUI {
        var ui = GetUI<T>();
        if (ui == null) return false;
        ui.Close();
        return true;
    }

    /// <summary>
    /// Get a user interface.
    /// </summary>
    /// <typeparam name="T">The type of user interface to get.</typeparam>
    /// <returns>The user interface of type T.</returns>
    public T GetUI<T>() where T : BaseUI {
        foreach (Transform child in canvas.transform) {
            if (child.TryGetComponent<T>(out var uiComponent)) {
                return uiComponent;
            }
        }

        return InstantiateUI<T>();
    }

    /// <summary>
    /// Instantiate a user interface.
    /// </summary>
    /// <typeparam name="T">The type of user interface to instantiate.</typeparam>
    /// <returns>The user interface of type T, or null if not found.</returns>
    public T InstantiateUI<T>() where T : BaseUI {
        foreach (var ui in uis) {
            if (ui is T) {
                var uiInstance = Instantiate(ui, canvas.transform);
                return (T)uiInstance;
            }
        }

        return null;
    }

    /// <summary>
    /// Destroy a user interface.
    /// </summary>
    /// <typeparam name="T">The type of user interface to destroy.</typeparam>
    public void DestroyUI<T>() where T : BaseUI {
        foreach (Transform child in canvas.transform) {
            if (child.TryGetComponent<T>(out var uiComponent)) {
                Destroy(child.gameObject);
            }
        }
    }
}
