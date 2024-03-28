using BulletParadise.Player;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Portal : MonoBehaviour, IInteractable
    {
        public bool IsFocused { get; set; }


        public new string name;

        [Header("Components")]
        public EnterInformationWindow enterInformationWindow;


        private void Start()
        {
            enterInformationWindow = PlayerController.Instance.canvasHandle.enterWindow;
        }

        public void Interact()
        {
            enterInformationWindow.OnEnter();
        }

        public void Focus()
        {
            IsFocused = true;
            enterInformationWindow.Setup(name);
        }

        public void LostFocus()
        {
            IsFocused = false;
            enterInformationWindow.Exit();
        }
    }
}
