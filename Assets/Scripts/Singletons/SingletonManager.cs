using UnityEngine;

public class SingletonManager : MonoBehaviour {
    /// <summary>
    /// The immediate parent directory of all the singleton prefabs.
    /// </summary>
    private const string SingletonsDirName = "Singletons";

    /// <summary>
    /// Create instances of all singletons.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() {
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Input Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Game Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/UI Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Save Manager"));
    }
}
