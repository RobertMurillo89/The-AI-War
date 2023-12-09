using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DataSerializer : MonoBehaviour
{
    public static void SerializeObject<T>(string filePath, T obj)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, obj);
        }
    }

    public static T DeserializeObject<T>(string filePath) 
    { 
        if (!File.Exists(filePath))
        {
            return default(T);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            return (T)formatter.Deserialize(fileStream);
        }
    }

}
