using BulletParadise.Entities.Items;
using BulletParadise.Misc;
using BulletParadise.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BulletParadise.UI
{
    public abstract class Slot : MonoBehaviour
    {
        protected PlayerController player;
        protected QuickBar quickBar;

        [Header("Components")]
        [SerializeField] private Image imageItem;
        [SerializeField] private TextMeshProUGUI keybindText;
        [SerializeField] protected TextMeshProUGUI amountText;

        [Header("Values")]
        public Key key;


        protected virtual void Start()
        {
            quickBar = GetComponentInParent<QuickBar>();
            player = PlayerController.Instance;
        }

        public void SetupKeybind(string path)
        {
            key = Utils.GetKey(path);

            string[] parts = path.Split('/');
            string keyName = parts[^1].Trim(']');
            keybindText.SetText(keyName.ToUpper());
        }

        public abstract void Use();

        public virtual void SetItem(Item item)
        {
            imageItem.color = Color.white;
            imageItem.sprite = item.sprite;
        }
        public virtual void Restart() { }

        protected void UpdateAmountText(int amount)
        {
            amountText.SetText(amount.ToString());
        }

        public void Clear()
        {
            imageItem.sprite = null;
            imageItem.color = new Color(0, 0, 0, 0);
        }
    }
}