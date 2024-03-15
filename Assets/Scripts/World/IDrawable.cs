namespace BulletParadise.World
{
    public interface IDrawable
    {
        bool CanDraw { get; }

        void Draw();
    }
}
