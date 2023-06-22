using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Singleton that manages saving and loading data from persistent data objects.
/// </summary>
public class SaveDataManager : Singleton<SaveDataManager> {
    /// <summary>
    /// The name of the file containing the save data.
    /// </summary>
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    /// <summary>
    /// The save data instance.
    /// </summary>
    private SaveData saveData;

    /// <summary>
    /// All the persistent data objects found on singleton initialization.
    /// </summary>
    private List<IDataPersistence> persistentDataObjects;

    /// <summary>
    /// Handles the actual saving and loading of data to and from disk.
    /// </summary>
    private SaveFileManager fileManager;

    /// <summary>
    /// The ID of the currently selected profile.
    /// </summary>
    private string selectedProfileId = "0";

    /// <inheritdoc />
    protected override void OnAwake() {
        fileManager = new SaveFileManager(Application.persistentDataPath, fileName);
        persistentDataObjects = GetPersistentDataObjects();
        LoadGame();
    }

    /// <inheritdoc />
    private void OnDestroy() {
        SaveGame();
    }

    /// <summary>
    /// Create a new default save data instance.
    /// </summary>
    public void NewGame() {
        saveData = new SaveData();
    }

    /// <summary>
    /// Fetch save data from disk and load it into all persistent data objects.
    /// </summary>
    public void LoadGame() {
        saveData = fileManager.Load(selectedProfileId);

        if (saveData == null) {
            Debug.LogError("No save data was found. Creating new save data.");
            NewGame();
        }

        foreach (var persistentDataObject in persistentDataObjects) {
            persistentDataObject.LoadData(saveData);
        }
    }

    /// <summary>
    /// Get all persistent data objects and save their data to disk.
    /// </summary>
    public void SaveGame() {
        foreach (var persistentDataObject in persistentDataObjects) {
            persistentDataObject.SaveData(saveData);
        }

        fileManager.Save(saveData, selectedProfileId);
    }

    /// <summary>
    /// Delete a profile.
    /// </summary>
    /// <param name="profileId">The ID of the profile to delete.</param>
    public void DeleteProfile(string profileId) {
        fileManager.Delete(profileId);
    }

    /// <summary>
    /// Get all persistent data objects in the scene.
    /// </summary>
    /// <returns>A list of all persistent data objects.</returns>
    private List<IDataPersistence> GetPersistentDataObjects() {
        var dataObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return dataObjects.ToList();
    }

    /// <summary>
    /// Get all profiles and their corresponding save data.
    /// </summary>
    /// <returns>A dictionary mapping a profile's ID to its save data.</returns>
    public Dictionary<string, SaveData> GetAllProfilesSaveData() {
        return fileManager.LoadProfiles();
    }
}
