using BulletParadise.Shooting;
using System.Collections;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "new Shotgun", menuName = "Weapons/WeaponType/Shotgun")]
    public class Shotgun : Weapon
    {
        public Weapon[] weapons;
        public float delayBetweenShots;


        public override IEnumerator Shoot(int layerMask, Transform shootingPosition, float shootingAngle)
        {
            int length = weapons.Length;
            for (int i = 0; i < length; i++)
            {
                var current = weapons[i];

                yield return current.Shoot(layerMask, shootingPosition, shootingAngle);
                yield return new WaitForSeconds(delayBetweenShots);
            }

            yield break;
        }
    }
}
