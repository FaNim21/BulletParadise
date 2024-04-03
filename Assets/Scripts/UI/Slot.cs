using BulletParadise.Entities;
using BulletParadise.Misc;
using BulletParadise.Shooting;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BulletParadise.UI
{
    public abstract class Slot : MonoBehaviour
    {
        protected QuickBar quickBar;

        [SerializeField] private Image imageItem;
        [SerializeField] private TextMeshProUGUI keybindText;

        public Item item;
        public Key key;


        private void Start()
        {
            quickBar = GetComponentInParent<QuickBar>();

            if (item != null)
                SetItem(item);
        }

        public void SetupKeybind(string path)
        {
            key = Utils.GetKey(path);

            string[] parts = path.Split('/');
            string keyName = parts[^1].Trim(']');
            keybindText.SetText(keyName.ToUpper());
        }

        public void SetItem(Item item)
        {
            if (quickBar.GetCurrentSelectedSlot().Equals(this) && item is Weapon weapon)
                quickBar.weapon = weapon;

            this.item = item;
            imageItem.color = Color.white;
            imageItem.sprite = item.sprite;
        }

        public abstract void Use();

        public virtual void Restart() { }

        public void Clear()
        {
            imageItem.sprite = null;
            imageItem.color = new Color(0, 0, 0, 0);
        }
    }
}