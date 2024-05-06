using BulletParadise.Misc;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletParadise.DataManagement
{
    //Without enryption
    public class SaveManager : MonoBehaviour
    {
        private List<ISavable> _savables = new();

        [Header("Data")]
        public GameData gameData;

        private string _savePath;


        private void Awake()
        {
            _savePath = Application.persistentDataPath + "/GameData.sav";

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var rootObjs = SceneManager.GetSceneAt(i).GetRootGameObjects();
                foreach (var root in rootObjs)
                {
                    _savables.AddRange(root.GetComponentsInChildren<ISavable>(true));
                }
            }

            Utils.Log("Obiekty wczytuj¹ce i zapisuj¹ce dane: " + _savables.Count);
        }
        private void Start()
        {
            LoadGame();
        }

        public void SaveGame()
        {
            //string json = JsonConvert.SerializeObject(objectSave, _jsonSettings);
            //File.WriteAllText(_savePath, json);

            foreach (var data in _savables)
            {
                data.Save(gameData);
            }
        }

        public void LoadGame()
        {
            foreach (var data in _savables)
            {
                data.Load(gameData);
            }

            string json = File.ReadAllText(_savePath);
            //JsonConvert.DeserializeObject<T>(json);

        }

        private void OnApplicationQuit() => SaveGame();
    }
}
