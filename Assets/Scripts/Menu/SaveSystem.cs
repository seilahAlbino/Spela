using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerInfoVar data = new PlayerInfoVar(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerInfoVar LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerInfoVar data = formatter.Deserialize(stream) as PlayerInfoVar;
            stream.Close();

            return data;
        }
        else
        {

            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
