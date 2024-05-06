using BulletParadise.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletParadise.DataManagement
{
    //Without enryption
    public class SaveManager : MonoBehaviour
    {
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

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var rootObjs = SceneManager.GetSceneAt(i).GetRootGameObjects();
                foreach (var root in rootObjs)
                {
                    _savables.AddRange(root.GetComponentsInChildren<ISavable>(true));
                }
            }

            Utils.Log("Savable objects count: " + _savables.Count);
        }
        private void Start()
        {
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
            }
        }
    }
}
