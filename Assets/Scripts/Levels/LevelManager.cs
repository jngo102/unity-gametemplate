using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistence {
    public void LoadData(SaveData saveData) {
        
    }

    public void SaveData(SaveData saveData) {
        saveData.LastScene = gameObject.scene.name;
    }
}
