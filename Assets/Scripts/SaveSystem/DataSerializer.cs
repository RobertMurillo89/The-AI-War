using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
//using System.Runtime.Serialization.Formatters.Binary;


public class DataSerializer
{

    private string dataDirPath = "";

    private string dataFileName = "";

    public DataSerializer(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public CharacterData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        CharacterData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //deserialize
                loadedData = JsonUtility.FromJson<CharacterData>(dataToLoad);
            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(CharacterData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to File: " + fullPath + "\n" + e);
        }
    }
    //public static void SerializeObject<T>(string filePath, T obj)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
    //    {
    //        formatter.Serialize(fileStream, obj);
    //    }
    //}

    //public static T DeserializeObject<T>(string filePath) 
    //{ 
    //    if (!File.Exists(filePath))
    //    {
    //        return default(T);
    //    }

    //    BinaryFormatter formatter = new BinaryFormatter();
    //    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
    //    {
    //        return (T)formatter.Deserialize(fileStream);
    //    }
    //}

}
