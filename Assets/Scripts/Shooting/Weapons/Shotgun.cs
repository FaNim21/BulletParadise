using BulletParadise.Shooting;
using System.Collections;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "new Shotgun", menuName = "Weapons/WeaponType/Shotgun")]
    public class Shotgun : Weapon
    {
        public float delayBetweenShots;


        public override void Shoot(int layerMask, Transform shootingPosition, float shootingAngle)
        {
            /*int length = weapons.Length;
            for (int i = 0; i < length; i++)
            {
                var current = weapons[i];

                //current.Shoot(layerMask, shootingPosition, shootingAngle);
                //yield return new WaitForSeconds(delayBetweenShots);
            }*/

        }
    }
}
