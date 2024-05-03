using BulletParadise.Entities;
using BulletParadise.Player;
using BulletParadise.Shooting;
using BulletParadise.World.Components;
using UnityEngine;

namespace BulletParadise
{
    public class ItemStand : MonoBehaviour, IInteractable
    {
        public bool IsFocused { get; set; }

        private StandController standController;

        [Header("Componenets")]
        public SpriteRenderer standSpriteRenderer;
        public SpriteRenderer itemSpriteRenderer;

        [Header("Values")]
        public Weapon weapon;


        private void Start()
        {
            standController = GetComponentInParent<StandController>();

            itemSpriteRenderer.color = Color.white;
            itemSpriteRenderer.sprite = weapon.sprite;
        }

        public void Interact(PlayerController player)
        {
            if (!IsFocused) return;
            standSpriteRenderer.color = Color.green;
            player.SetWeapon(weapon);
        }

        public void Focus()
        {
            if (IsFocused) return;
            IsFocused = true;

            standSpriteRenderer.color = Color.yellow;

            standController.ShowWeaponInfo(weapon);
        }

        public void LostFocus()
        {
            if (!IsFocused) return;
            IsFocused = false;

            standSpriteRenderer.color = Color.white;

            standController.CloseWeaponInfo();
        }
    }
}