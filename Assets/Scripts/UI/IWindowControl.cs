
namespace BulletParadise.UI
{
    public interface IWindowControl
    {
        CanvasHandle UIHandle { get; }
        bool IsActive { get; }

        void ToggleWindow();
    }
}
