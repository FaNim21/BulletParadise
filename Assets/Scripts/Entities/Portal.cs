using BulletParadise.Player;
using BulletParadise.World.Components;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Portal : MonoBehaviour, IInteractable
    {
        public bool IsFocused { get; set; }

        public new string name;
        private PortalsController controller;


        private void Awake()
        {
            controller = GetComponentInParent<PortalsController>();
        }

        public void Focus()
        {
            IsFocused = true;
            controller.ShowPortalWindow(this);
        }

        public void LostFocus()
        {
            IsFocused = false;
            controller.HidePortalWindow();
        }

        public void Interact(PlayerController player)
        {
            controller.EnterPortal();
        }
    }
}
