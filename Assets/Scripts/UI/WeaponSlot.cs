using BulletParadise.Entities.Items;
using BulletParadise.Shooting;

namespace BulletParadise.UI
{
    public class WeaponSlot : Slot
    {
        public Weapon weapon;


        protected override void Start()
        {
            base.Start();

            if (weapon != null) SetItem(weapon);
        }

        public override void Use()
        {

        }

        public override void SetItem(Item item)
        {
            if (item is not Weapon weapon) return;
            this.weapon = weapon;

            if (quickBar.GetCurrentSelectedSlot().Equals(this))
                quickBar.weapon = weapon;

            base.SetItem(item);
        }
    }
}