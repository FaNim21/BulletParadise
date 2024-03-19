
namespace BulletParadise.UI
{
    public interface IWindowControl
    {
        bool IsActive { get; }

        void ToggleWindow();

        void Open();
        void Close();
    }
}
