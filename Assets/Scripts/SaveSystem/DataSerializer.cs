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

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "Hope";

    public DataSerializer(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
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

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
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

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

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

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
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
