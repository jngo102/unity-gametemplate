using System.Collections.Generic;

public static class SceneData {
    private static List<string> NonGameplayScenes = new() {
        "MainMenu",
    };

    public static bool IsGameplayScene(string sceneName) {
        return !NonGameplayScenes.Contains(sceneName);
    }
}
