using UnityEngine;

public class SingletonManager : MonoBehaviour {
    private const string SingletonsDirName = "Singletons";

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() {
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Game Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Input Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/UI Manager"));
        Instantiate(Resources.Load<GameObject>($"{SingletonsDirName}/Save Manager"));
    }
}
