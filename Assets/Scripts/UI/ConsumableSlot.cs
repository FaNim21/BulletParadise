using BulletParadise.Entities.Items;
using UnityEngine;

namespace BulletParadise.UI
{
    public class ConsumableSlot : Slot
    {
        public Consumable consumable;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int amount;


        protected override void Start()
        {
            base.Start();

            if (consumable != null) SetItem(consumable);
        }

        public override void Use()
        {
            if ((amount - 1) < 0) return;
            if (!consumable.Consume(player)) return;

            amount--;
            UpdateAmountText(amount);
        }

        public override void SetItem(Item item)
        {
            if (item is not Consumable consumable) return;
            this.consumable = consumable;
            Restart();

            base.SetItem(item);
        }

        public override void Restart()
        {
            if (consumable == null) return;

            amount = consumable.maxCount;
            UpdateAmountText(amount);
        }
    }
}