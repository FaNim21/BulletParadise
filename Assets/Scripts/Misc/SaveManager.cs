using BulletParadise.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletParadise.DataManagement
{
    //Without enryption(not needed)
    public class SaveManager : MonoBehaviour
    {
        public GameData GameData { get { return gameData; } }

        private GameManager gameManager;

        private List<ISavable> _savables = new();

        [Header("Data")]
        [SerializeField] private GameData gameData;

        [Header("Debug")]
        [SerializeField] private bool loadedCorrectly;

        private string _savePath;


        private void Awake()
        {
            gameManager = GetComponent<GameManager>();

            gameData = new();
            _savePath = Application.persistentDataPath + "/GameData.sav";
        }

        public void FindAllSavableObjects()
        {
            _savables.Clear();
            _savables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToList();

            Utils.Log("Savable objects count: " + _savables.Count);
            foreach (var data in _savables)
                Utils.Log("savable object name: " + data.GetType());

            LoadGame();
        }

        public void SaveGame()
        {
            foreach (var data in _savables)
            {
                data.Save(gameData);
            }

            Save();
        }

        public void LoadGame()
        {
            Load();

            if (!loadedCorrectly)
            {
                Utils.LogError("Couldn't load save...");
                return;
            }

            foreach (var data in _savables)
            {
                data.Load(gameData);
            }

            Utils.LogWarning("Loaded data successfully");
        }

        private void OnApplicationQuit() => SaveGame();

        private void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_savePath));
                string dataToStore = JsonUtility.ToJson(gameData, true);

                using (FileStream stream = new(_savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError("Error while saving data: " + ex.ToString());
            }
        }
        private void Load()
        {
            try
            {
                string dataToLoad = string.Empty;
                using (FileStream stream = new(_savePath, FileMode.Open))
                {
                    using (StreamReader reader = new(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                gameData = JsonUtility.FromJson<GameData>(dataToLoad);
                loadedCorrectly = true;
            }
            catch (Exception ex)
            {
                Utils.LogError("Error while loading data: " + ex.ToString());
                loadedCorrectly = false;
            }
        }
    }
}
