using UnityEngine;

namespace BulletParadise.Entities.Items
{
    [CreateAssetMenu(fileName = "new HealthPotion", menuName = "Items/HealthPotion")]
    public class HealPotion : Consumable
    {
        public int healAmount;

        public override bool Consume(Entity entity)
        {
            return entity.healthManager.Heal(healAmount);
        }
    }
}
