using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileManager {
    private string dataDirPath = "";
    private string dataFileName = "";

    public SaveFileManager(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public SaveData Load(string profileId) {
        var fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        SaveData loadedData = null;
        if (File.Exists(fullPath)) {
            try {
                var loadedJson = "";
                using (var stream = new FileStream(fullPath, FileMode.Open)) {
                    using (var reader = new StreamReader(stream)) {
                        loadedJson = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<SaveData>(loadedJson);
            } catch (Exception e) {
                Debug.LogError($"Error occurred while trying ot load data from file: {fullPath}\n{e}");
                throw;
            }
        }

        return loadedData;
    }

    public void Save(SaveData data, string profileId) {
        var fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            var dataJson = JsonUtility.ToJson(data, true);
            using var stream = new FileStream(fullPath, FileMode.Create);
            using var writer = new StreamWriter(stream);
            writer.Write(dataJson);
        } catch (Exception e) {
            Debug.LogError($"Failed to save data to {fullPath}: {e}");
            throw;
        }
    }

    public void Delete(string profileId) {
        if (profileId == null) return;

        var fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try {
            if (File.Exists(fullPath)) {
                Directory.Delete(Path.GetDirectoryName(fullPath)!, true);
            } else {
                Debug.LogWarning($"Tried to delete save data, but data was not found at {fullPath}");
            }
        } catch (Exception e) {
            Debug.LogError($"Tried to delete save file for profile {profileId} at {fullPath}\n{e}");
        }
    }

    public Dictionary<string, SaveData> LoadProfiles() {
        var profileDict = new Dictionary<string, SaveData>();
        var dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (var dirInfo in dirInfos) {
            var profileId = dirInfo.Name;
            var fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"Skipping directory {profileId} when loading all profiles because it does not containing save data.");
                continue;
            }

            var profileData = Load(profileId);
            if (profileData != null) {
                profileDict.Add(profileId, profileData);
            } else {
                Debug.LogError($"Error occured when trying to load profile {profileId}");
            }
        }
        return profileDict;
    }
}
