using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager {
    private string saveFolderPath;

    public void Setup() {
        // cannot be called in constructor
        saveFolderPath = $"{Application.persistentDataPath}/saves";
    }

    public bool Save(string saveName, object saveData) {
        var formatter = GetBinaryFormatter();

        if (!Directory.Exists(saveFolderPath)) {
            Directory.CreateDirectory(saveFolderPath);
        }

        var filePath = GetFilePath(saveName);
        var file = File.Create(filePath);
        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }

    public object Load(string saveName) {
        var filePath = GetFilePath(saveName);

        if (!File.Exists(filePath)) return null;

        var formatter = GetBinaryFormatter();
        var file = File.Open(filePath, FileMode.Open);

        try {
            var obj = formatter.Deserialize(file);
            file.Close();

            return obj;
        }
        catch {
            Debug.LogErrorFormat("Failed to load file at {0}", filePath);

            file.Close();

            return null;
        }
    }

    private string GetFilePath(string saveName) {
        return $"{saveFolderPath}/{saveName}.save";
    }

    private BinaryFormatter GetBinaryFormatter() {
        var formatter = new BinaryFormatter();

        return formatter;
    }
}
