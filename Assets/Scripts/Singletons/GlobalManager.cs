using UnityEngine;

public class GlobalManager : MonoBehaviour {
    /// <summary>
    /// The immediate parent directory of all the singleton prefabs.
    /// </summary>
    private const string SingletonsDirName = "Singletons";

    /// <summary>
    /// Create instances of all singletons.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() {
        InstantiateSingletons();
        GetSingletons();
    }

    /// <summary>
    /// Create instances of all singletons.
    /// </summary>
    private static void InstantiateSingletons() {
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Input Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Game Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/UI Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Save Manager"));
    }

    /// <summary>
    /// Get all singletons.
    /// </summary>
    private static void GetSingletons() {
        var inputManager = InputManager.Instance;
        var gameManager = GameManager.Instance;
        var uiManager = UIManager.Instance;
        var saveManager = SaveDataManager.Instance;
    }
}
