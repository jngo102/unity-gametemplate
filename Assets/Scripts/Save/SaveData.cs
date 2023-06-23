using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Data structure containing all objects with persistent data.
/// </summary>
[Serializable]
public class SaveData {
    /// <summary>
    /// Input binding overrides that the player has made.
    /// </summary>
    public string BindingOverrides;

    /// <summary>
    /// The game's default language.
    /// </summary>
    public string Language;
    
    /// <summary>
    /// The last scene that the player saved at.
    /// </summary>
    public string LastScene;
    
    public SaveData() {
        BindingOverrides = "";
        Language = "en";
        LastScene = "SampleScene";
    }
}
