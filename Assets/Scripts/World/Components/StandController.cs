using BulletParadise.Shooting;
using BulletParadise.UI;
using UnityEngine;

namespace BulletParadise
{
    public class StandController : MonoBehaviour
    {
        [Header("Components")]
        public WeaponInformationWindow weaponInformationWindow;


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