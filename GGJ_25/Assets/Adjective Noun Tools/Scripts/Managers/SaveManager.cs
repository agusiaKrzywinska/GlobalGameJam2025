using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANT.ScriptableProperties;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ANT
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField, Tooltip("The current version of the save file")]
        private string versionNumber = "0.0000001";
        [SerializeField, Tooltip("the additional path to save it at. it assumes Persistent Data Path")]
        private string path = "/mySave.save";
        [SerializeField, Tooltip("The current version of the save file")]
        private ScriptableInt[] scriptableInts;
        [SerializeField, Tooltip("The current version of the save file")]
        private ScriptableBool[] scriptableBools;
        [SerializeField, Tooltip("The current version of the save file")]
        private ScriptableString[] scriptableStrings;

        /// <summary>
        /// the Save data. 
        /// </summary>
        private SaveData currentSave;

        /// <summary>
        /// Will copy all the data in the game to the file to save it. 
        /// </summary>
        public void WriteSaveData()
        {
            currentSave = new SaveData();
            currentSave.versionNumber = versionNumber;

            //Generic Scriptable Objects
            for (int i = 0; i < scriptableInts.Length; i++)
            {
                currentSave.scriptableInts.Add(scriptableInts[i].GetValue());
            }
            for (int i = 0; i < scriptableBools.Length; i++)
            {
                currentSave.scriptableBools.Add(scriptableBools[i].GetValue());
            }
            for (int i = 0; i < scriptableStrings.Length; i++)
            {
                currentSave.scriptableStrings.Add(scriptableStrings[i].GetValue());
            }

            //Writing Data
            string destination = Application.persistentDataPath + path;
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, currentSave);
            file.Close();

            Debug.Log(destination);
            Debug.Log("Saved File:" + currentSave.ToString());
        }

        /// <summary>
        /// Will read in all the save information in the file to replace the values in game. 
        /// </summary>
        public void ReadSaveData()
        {
            string destination = Application.persistentDataPath + path;
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                Debug.LogWarning("File not found");
                WriteSaveData();
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            currentSave = (SaveData)bf.Deserialize(file);
            file.Close();

            if (versionNumber != currentSave.versionNumber)
            {
                Debug.LogError("Save versions do not match");
                return;
            }


            Debug.Log("Loaded File:" + currentSave.ToString());

            for (int i = 0; i < scriptableInts.Length; i++)
            {
                scriptableInts[i].SetValue(currentSave.scriptableInts[i]);
            }

            for (int i = 0; i < scriptableBools.Length; i++)
            {
                scriptableBools[i].SetValue(currentSave.scriptableBools[i]);
            }

            for (int i = 0; i < scriptableStrings.Length; i++)
            {
                scriptableStrings[i].SetValue(currentSave.scriptableStrings[i]);
            }

        }

        /// <summary>
        /// Deletes the save file. 
        /// </summary>
        public void DeleteSaveData()
        {
            string destination = Application.persistentDataPath + path;

            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
        }
    }

    [System.Serializable]
    public class SaveData
    {
        /// <summary>
        /// What is the save version.
        /// </summary>
        public string versionNumber;

        /// <summary>
        /// All the ints that are currently saved. 
        /// </summary>
        public List<int> scriptableInts = new List<int>();
        
        /// <summary>
        /// All the bools that are currently saved. 
        /// </summary>
        public List<bool> scriptableBools = new List<bool>();
        
        /// <summary>
        /// all the strings that are currently saved. 
        /// </summary>
        public List<string> scriptableStrings = new List<string>();

        /// <summary>
        /// prints a legible version of the save data. 
        /// </summary>
        /// <returns>a legible string of the the save data.</returns>
        public override string ToString()
        {
            string temp = "";
            temp += versionNumber + "\n";
            temp += "Scriptable Ints:";
            for (int i = 0; i < scriptableInts.Count; i++)
            {
                temp += $" {scriptableInts[i]}";
            }
            temp += "\n";
            temp += "Scriptable Bools:";
            for (int i = 0; i < scriptableBools.Count; i++)
            {
                temp += $" {scriptableBools[i]}";
            }
            temp += "\n";
            temp += "Scriptable Strings:";
            for (int i = 0; i < scriptableStrings.Count; i++)
            {
                temp += $" {scriptableStrings[i]}";
            }

            return temp;
        }

        /// <summary>
        /// Converts an int to a bool 
        /// </summary>
        /// <param name="value">the int value you want to convert.</param>
        /// <returns>a bool converted from int.</returns>
        public static bool GetBool(int value)
        {
            return value == 1;
        }

        /// <summary>
        /// Converts a bool in to an int. 
        /// </summary>
        /// <param name="value">the bool value you want to convert.</param>
        /// <returns>an int converted from a bool.</returns>
        public static int GetInt(bool value)
        {
            return value ? 1 : 0;
        }
    }
}