using Newtonsoft.Json.Linq;
using System;

[Serializable]
public class SaveData {
    public string Bindings;
    public SaveData() {
        Bindings = "";
    }
}
