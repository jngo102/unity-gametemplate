using UnityEngine;

/// <summary>
/// Location that the player may save their game at.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SaveSpot : MonoBehaviour, IDataPersistence {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Save(other.GetComponent<Player>());
        }
    }

    /// <inheritdoc />
    public void LoadData(SaveData saveData) {
        
    }

    /// <inheritdoc />
    public void SaveData(SaveData saveData) {
        saveData.SaveScene = gameObject.scene.name;
    }

    /// <summary>
    /// Save the game at the save spot.
    /// </summary>
    /// <param name="player">The player who saved at the save spot.</param>
    public void Save(Player player) {
        player.GetComponent<HealthManager>().FullHeal();
        SaveDataManager.Instance.SaveGame();
    }
}
