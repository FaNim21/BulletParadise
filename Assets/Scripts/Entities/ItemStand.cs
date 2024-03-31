using BulletParadise.Entities;
using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise
{
    public class ItemStand : MonoBehaviour, IInteractable
    {
        public bool IsFocused { get; set; }

        [Header("Componenets")]
        public SpriteRenderer standSpriteRenderer;
        public SpriteRenderer itemSpriteRenderer;

        [Header("Values")]
        public Weapon weapon;


        private void Start()
        {
            itemSpriteRenderer.color = Color.white;
            itemSpriteRenderer.sprite = weapon.sprite;
        }

        public void Interact()
        {
            if (!IsFocused) return;
            standSpriteRenderer.color = Color.green;
        }

        public void Focus()
        {
            if (IsFocused) return;
            IsFocused = true;

            standSpriteRenderer.color = Color.yellow;
        }

        public void LostFocus()
        {
            if (!IsFocused) return;
            IsFocused = false;

            standSpriteRenderer.color = Color.white;
        }
    }
}