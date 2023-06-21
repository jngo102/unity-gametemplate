using System;

/// <summary>
/// Data structure containing all objects with persistent data.
/// </summary>
[Serializable]
public class SaveData {
    /// <summary>
    /// Input binding overrides that the player has made.
    /// </summary>
    public string BindingOverrides;

    public SaveData() {
        BindingOverrides = "";
    }
}
