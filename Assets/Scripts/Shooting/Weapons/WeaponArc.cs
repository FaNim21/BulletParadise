using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponArc", menuName = "Weapons/WeaponType/Arc")]
    public class WeaponArc : Weapon
    {
        public float angle;


        public override void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle)
        {
            float degree;
            int length = projectiles.Length;
            int halfLength = length / 2;

            for (int i = 1; i <= length; i++)
            {
                var current = projectiles[i - 1];

                //degree += angle;

                //to mozna uproscic jednym dzialaniem bez ifa
                //degree = shootingAngle + (angle * (halfLength - (i - 1))) - (angle / 2) ;
                degree = shootingAngle + (angle * (halfLength - (i - 1))) - (angle / 2) - (angle / 2 * (length % 2 * -1));
                /*if (length % 2 == 0) degree = shootingAngle + (angle * (halfLength - (i - 1))) - (angle / 2);
                else degree = shootingAngle + (angle * (halfLength - (i - 1)));*/

                Quaternion quaternionAngle = Quaternion.Euler(0, 0, degree);
                var projectile = Instantiate(GameManager.Projectile, shootingPosition, quaternionAngle);
                ProjectileBehavior behavior = current.GenerateBehavior(projectile.rb, quaternionAngle * Vector2.right);
                projectile.Setup(layerMask, behavior);
            }
        }
    }
}