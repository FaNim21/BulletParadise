using BulletParadise.DataManagement;
using BulletParadise.Entities.Items;
using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.UI
{
    public class WeaponSlot : Slot, ISavable
    {
        public Weapon weapon;
        [SerializeField] private int slotIndex;


        protected override void Start()
        {
            base.Start();

            if (weapon != null)
            {
                quickBar.SetCurrentChosen(slotIndex);
                SetItem(weapon);
            }
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

        public void Save(GameData gameData)
        {
            if (weapon == null || weapon.id < 0) return;

            gameData.weaponsIdsInSlot[slotIndex] = weapon.id;
        }
        public void Load(GameData gameData)
        {
            int itemID = gameData.weaponsIdsInSlot[slotIndex];
            if (itemID == -1) return;

            Weapon weapon = (Weapon)GameManager.Instance.GetItemFromID(itemID);
            if (weapon == null) return;

            SetItem(weapon);
        }
    }
}