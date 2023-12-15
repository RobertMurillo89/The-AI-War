using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataSerializer
{
    // Path to the directory where data files are stored
    private string dataDirPath = "";
    // Name of the file used for storing data
    private string dataFileName = "";
    // Flag to determine whether encryption should be used
    private bool useEncryption = false;
    // A passphrase for simple XOR encryption/decryption
    private readonly string encryptionCodeWord = "Hope";

    // Constructor initializes the serializer with the directory path, file name, and encryption flag
    public DataSerializer(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    // Loads character data for a given profile ID
    public CharacterData Load(string profileId)
    {
        // Combine directory path, profile ID, and file name to create a full path to the data file
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        // Placeholder for the loaded character data
        CharacterData loadedData = null;

        // Check if the data file exists at the full path
        if (File.Exists(fullPath))
        {
            try
            {
                // String to store the loaded data
                string dataToLoad = "";

                // Open a file stream for reading the data file
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // Create a stream reader to read the data from the file
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // Read all the data from the file
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // If encryption is enabled, decrypt the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // Deserialize the JSON string back into a CharacterData object
                loadedData = JsonUtility.FromJson<CharacterData>(dataToLoad);
            }
            catch (Exception e) 
            {
                // Log any errors that occurred during the loading process
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        // Return the loaded (or null if not found or error occurred) character data
        return loadedData;
    }

    // Saves character data for a given profile ID
    public void Save(CharacterData data, string profileId)
    {
        // Combine directory path, profile ID, and file name to create a full path to the data file
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            // Ensure that the directory exists where the data file will be saved
            var directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            // Check if file exists and is not being used
            if (!IsFileInUse(fullPath))
            {
                // Serialize the CharacterData object to a JSON string
                string dataToStore = JsonUtility.ToJson(data, true);

                // If encryption is enabled, encrypt the data
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }
                // Locking mechanism to handle concurrent access
                lock (this)
                {
                    // Open a file stream for writing the data file
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        // Create a stream writer to write the data to the file

                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            // Write the (possibly encrypted) data to the file
                            writer.Write(dataToStore);
                        }
                    }
                }
            }

            else
            {
                // If the file is in use, log a warning
                Debug.LogWarning("File is in use: " + fullPath);
            }
        }
        catch (Exception e)
        {
            // Log any errors that occurred during the save process
            Debug.LogError("Error occured when trying to save data to File: " + fullPath + "\n" + e);
        }
    }

    private bool IsFileInUse(string filePath)
    {
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            return true;
        }

        return false;
    }

    // Simple XOR encryption/decryption to obfuscate the data
    private string EncryptDecrypt(string data)
    {
        // String to store the modified data
        string modifiedData = "";

        // Go through each character in the data
        for (int i = 0; i < data.Length; i++)
        {
            // XOR the character with a character from the code word to encrypt/decrypt it
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        // Return the modified (encrypted/decrypted) data
        return modifiedData;
    }

    // Loads character data for all profiles found in the data directory
    public Dictionary<string, CharacterData> LoadAllProfiles()
    {
        // Dictionary to hold the character data for each profile ID
        Dictionary<string, CharacterData> profileDictionary = new Dictionary<string, CharacterData>();

        // Get information about all directories in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            // Use the directory name as the profile ID
            string profileId = dirInfo.Name;

            // Combine directory path, profile ID, and file name to create a full path to the data file
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

            // Check if the data file exists for this profile
            if (!File.Exists(fullPath))
            {
                // If the file doesn't exist, log a warning and skip this directory
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " 
                    + profileId);
                continue;
            }

            // Load the character data for this profile
            CharacterData profileData = Load(profileId);

            // If data was successfully loaded, add it to the dictionary
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                // If there was an error loading the data, log an error
                Debug.LogError("Tried to load profile but something went terribly wrong. ProfileId: " + profileId);
            }
        }
        // Return the dictionary containing all loaded profile data
        return profileDictionary;
    }

}
