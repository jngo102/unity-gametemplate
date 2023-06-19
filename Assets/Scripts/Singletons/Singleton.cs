using JetBrains.Annotations;
using UnityEngine;

public abstract class Singleton : MonoBehaviour {
    public static bool Quitting { get; private set; }

    private void OnApplicationQuit() {
        Quitting = true;
    }
}

public abstract class Singleton<T> : Singleton where T : MonoBehaviour {
    [CanBeNull] private static T _instance;
    [NotNull] private static readonly object Lock = new();
    [SerializeField] private bool _persistent = true;

    [NotNull]
    public static T Instance {
        get {
            if (Quitting) {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                // ReSharper disable once AssignNullToNotNullAttribute
                return null;
            }

            lock (Lock) {
                if (_instance != null) return _instance;
                var instances = FindObjectsOfType<T>(true);
                var count = instances.Length;
                if (count > 0) {
                    if (count == 1) return _instance = instances[0];
                    Debug.LogWarning(
                        $"[{typeof(T)}] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++) Destroy(instances[i]);
                    return _instance = instances[0];
                }

                Debug.Log($"[{typeof(T)}] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                return _instance = new GameObject($"{typeof(T)}").AddComponent<T>();
            }
        }
    }

    private void Awake() {
        if (_persistent) DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    protected virtual void OnAwake() { }
}
