using System;

[Serializable]
public class SaveData {
    public int CurrentCells;
    public int MaxCells;

    public SaveData() {
        CurrentCells = 5;
        MaxCells = 5;
    }
}
