using UnityEngine;

/// <summary>
/// Global manager that initializes all singletons.
/// </summary>
public class GlobalManager : MonoBehaviour {
    /// <summary>
    /// The immediate parent directory of all the singleton prefabs.
    /// </summary>
    private const string SingletonsDirName = "Singletons";

    /// <summary>
    /// Initialize is run on game start.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() {
        InstantiateSingletons();
    }

    /// <summary>
    /// Create instances of all singletons.
    /// </summary>
    private static void InstantiateSingletons() {
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/GameManager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/UIManager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/SaveDataManager"));
#if UNITY_EDITOR
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/UnityExplorer"));
#endif
    }
}
