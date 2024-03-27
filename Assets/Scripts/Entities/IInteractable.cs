namespace BulletParadise.Entities
{
    public interface IInteractable
    {
        bool IsFocused { get; set; }

        void Interact();
        void Focus();
        void LostFocus();
    }
}