using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour {
    private static string savePath => Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(GameData data) {
        string json = JsonUtility.ToJson(data, true);

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            PlayerPrefs.SetString("SaveData", json);
            PlayerPrefs.Save();
        } else {
            File.WriteAllText(savePath, json);
        }
    }

    public static GameData LoadGame() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            if (!PlayerPrefs.HasKey("SaveData")) return null;
            string json = PlayerPrefs.GetString("SaveData");
            return JsonUtility.FromJson<GameData>(json);
        } else {
            if (!File.Exists(savePath)) return null;
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
    }
}
