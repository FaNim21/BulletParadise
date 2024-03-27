using BulletParadise.Entities;
using UnityEngine;

namespace BulletParadise
{
    public class ItemStand : MonoBehaviour, IInteractable
    {
        public bool IsFocused { get; set; }

        public SpriteRenderer standSpriteRenderer;


        public void Focus()
        {
            standSpriteRenderer.color = Color.yellow;
        }

        public void Interact()
        {
            standSpriteRenderer.color = Color.green;
        }

        public void LostFocus()
        {
            standSpriteRenderer.color = Color.white;
        }
    }
}