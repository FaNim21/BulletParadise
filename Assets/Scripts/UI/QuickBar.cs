using BulletParadise.Player;
using BulletParadise.Shooting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace BulletParadise.UI
{
    public class QuickBar : MonoBehaviour
    {
        public WeaponSlot[] weaponSlots;
        public ConsumableSlot[] consumableSlots;

        [Header("Objects")]
        public Transform selectTransform;

        [Header("Components")]
        public InputActionAsset playerInput;

        [Header("Debug")]
        [ReadOnly] public Weapon weapon;

        private int currentChosen = -1;


        private void Start()
        {
            weaponSlots[0].SetupKeybind(playerInput.actionMaps[0].actions[2].ToString());
            weaponSlots[1].SetupKeybind(playerInput.actionMaps[0].actions[3].ToString());

            consumableSlots[0].SetupKeybind(playerInput.actionMaps[0].actions[4].ToString());
            consumableSlots[1].SetupKeybind(playerInput.actionMaps[0].actions[5].ToString());

            SelectSlot(0);
        }

        public void SetSelection(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;

            Key key = ((KeyControl)context.control).keyCode;

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i].key == key) SelectSlot(i);
            }

            for (int i = 0; i < consumableSlots.Length; i++)
            {
                var current = consumableSlots[i];
                if (current.key == key) current.Use();
            }
        }

        private void SelectSlot(int index)
        {
            var slot = weaponSlots[index];

            weapon = slot.weapon;
            currentChosen = index;

            selectTransform.position = slot.transform.position;
            selectTransform.gameObject.SetActive(true);
            PlayerController.Instance.shootingManager.Restart();
        }

        public void SetWeaponToSlot(Weapon weapon)
        {
            GetCurrentSelectedSlot()?.SetItem(weapon);
        }

        public WeaponSlot GetCurrentSelectedSlot()
        {
            if (currentChosen == -1) return null;
            return weaponSlots[currentChosen];
        }

        public void ResetSlots()
        {
            for (int i = 0; i < consumableSlots.Length; i++)
            {
                consumableSlots[i].Restart();
            }
        }

        public void SetCurrentChosen(int index) => currentChosen = index;
    }
}