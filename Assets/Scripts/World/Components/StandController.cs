using BulletParadise.Shooting;
using BulletParadise.UI;
using UnityEngine;

namespace BulletParadise.World.Components
{
    public class StandController : MonoBehaviour
    {
        private WeaponInformationWindow weaponInformationWindow;


        private void Awake()
        {
            weaponInformationWindow = GetComponentInChildren<WeaponInformationWindow>();
        }

        public void ShowWeaponInfo(Weapon weapon)
        {
            weaponInformationWindow.Setup(weapon);
        }

        public void CloseWeaponInfo()
        {
            weaponInformationWindow.Exit();
        }
    }
}