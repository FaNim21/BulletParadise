namespace BulletParadise.DataManagement
{
    public interface ISavable
    {
        void Save(GameData gameData);
        void Load(GameData gameData);
    }
}
