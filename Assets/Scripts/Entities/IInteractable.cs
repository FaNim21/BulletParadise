using BulletParadise.Player;

namespace BulletParadise.Entities
{
    public interface IInteractable
    {
        bool IsFocused { get; set; }

        void Interact(PlayerController player);
        void Focus();
        void LostFocus();
    }
}