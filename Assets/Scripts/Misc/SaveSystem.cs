using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace BulletParadise.Misc
{
    //Without enryption
    public static class SaveSystem
    {
        private static string _savePath = Application.persistentDataPath + "/GameData.json";
        private static JsonSerializerSettings _jsonSettings = new() { Formatting = Formatting.Indented };

        public static void Save<T>(T objectSave)
        {
            string json = JsonConvert.SerializeObject(objectSave, _jsonSettings);
            File.WriteAllText(_savePath, json);
        }

        public static T Load<T>()
        {
            string json = File.ReadAllText(_savePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
