using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Linq;

/// <summary>
/// Controller for the language dropdown setting.
/// </summary>
public class LanguageSetting : MonoBehaviour, IDataPersistence {
    /// <summary>
    /// The dropdown containing an array of language locales.
    /// </summary>
    [SerializeField] private TMP_Dropdown dropdown;

    /// <inheritdoc />
    public void LoadData(SaveData saveData) {
        var options = dropdown.options;
        var index = options.IndexOf(options.First(option => option.text == saveData.Language));
        dropdown.value = index;
    }

    /// <inheritdoc />
    public void SaveData(SaveData saveData) {
        saveData.Language = dropdown.options[dropdown.value].text;
    }

    /// <summary>
    /// Set the current locale to that of the dropdown's value.
    /// </summary>
    public void SetLanguage() {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(dropdown.options[dropdown.value].text);
        SaveDataManager.Instance.SaveGame();
    }
}
