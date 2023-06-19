using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager> {
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private SaveData saveData;
    private List<IDataPersistence> persistentDataObjects;
    private SaveFileManager fileManager;

    private string selectedProfileId = "0";

    private void Awake() {
        fileManager = new SaveFileManager(Application.persistentDataPath, fileName);
        persistentDataObjects = GetPersistentDataObjects();
        LoadGame();
    }

    public void NewGame() {
        saveData = new SaveData();
    }

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

    public void SaveGame() {
        foreach (var persistentDataObject in persistentDataObjects) {
            persistentDataObject.SaveData(saveData);
        }

        fileManager.Save(saveData, selectedProfileId);
    }

    public void DeleteProfile(string profileId) {
        fileManager.Delete(profileId);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPersistence> GetPersistentDataObjects() {
        var dataObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataObjects);
    }

    public Dictionary<string, SaveData> GetAllProfilesSaveData() {
        return fileManager.LoadProfiles();
    }
}
