namespace BulletParadise.Entities.Items
{
    public abstract class Consumable : Item
    {
        public int maxCount;

        public abstract bool Consume(Entity entity);
    }
}
