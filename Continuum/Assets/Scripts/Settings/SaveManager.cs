using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    private static readonly string savePath = Application.persistentDataPath + "/player.bin";
    
    public static void SaveData(PlayerController pc, int level)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(savePath, FileMode.Create);
        PlayerData data = new(pc, level);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved Level" + data.level);
        Debug.Log("Saved data to " + savePath);
    }

    public static PlayerData LoadData()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(savePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            Debug.Log("Loaded data from " + savePath);

            return data;
        }
        else
        {
            Debug.Log("Savefile not found in " + savePath);
            return null;
        }
    }
}
