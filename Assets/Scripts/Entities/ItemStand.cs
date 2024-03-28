using BulletParadise.Entities;
using UnityEngine;

namespace BulletParadise
{
    public class ItemStand : MonoBehaviour, IInteractable
    {
        public bool IsFocused { get; set; }

        public SpriteRenderer standSpriteRenderer;


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