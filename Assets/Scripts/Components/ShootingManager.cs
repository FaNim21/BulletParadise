using BulletParadise.Datas;
using BulletParadise.Shooting;
using System;
using System.Collections;
using UnityEngine;

namespace BulletParadise.Components
{
    [System.Serializable]
    public struct WeaponShootBossData
    {
        public Weapon weapon;
        public BossShootAnimData animData;
        public string animTrigger;
    }

    public class ShootingManager : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] public Transform shootingOffset;

        public int layerMask;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isShooting;
        private bool _canShoot = true;

        private Coroutine _shootingCoroutine;
        private Coroutine _restartShootingCoroutine;


        private void Awake()
        {
            _animator = transform.Find("Body").GetComponent<Animator>();
        }

        public void Shoot(Weapon weapon, float angle, Action OnShoot = null, string animTriggerName = "")
        {
            if (_isShooting || weapon == null || !_canShoot) return;

            OnShoot?.Invoke();

            StartCoroutine(ShootCoroutine(weapon, angle, animTriggerName));
        }
        public void Shoot(WeaponShootBossData data, float angle, Action OnShoot = null)
        {
            if (_isShooting || data.weapon == null || !_canShoot) return;
            OnShoot?.Invoke();

            if (data.animData == null)
            {
                StartCoroutine(ShootCoroutine(data.weapon, angle));
                return;
            }
            StartCoroutine(ShootingBossCoroutine(data, angle));
        }

        private IEnumerator ShootCoroutine(Weapon weapon, float angle, string animTriggerName = null)
        {
            _isShooting = true;

            float shotSpeed = Mathf.Max(weapon.frequency / 3f, 1f);
            StartShootingAnimation(shotSpeed, animTriggerName);

            weapon.Shoot(layerMask, shootingOffset, angle);
            yield return new WaitForSeconds(1f / weapon.frequency);

            _isShooting = false;
        }
        private IEnumerator ShootingBossCoroutine(WeaponShootBossData data, float angle)
        {
            _isShooting = true;

            float shootSpeed = Mathf.Max(data.weapon.frequency, 1f);
            float scaledDelay = data.animData.shootDelay / shootSpeed;
            float scaledRestTime = (data.animData.clip.length / shootSpeed) - scaledDelay;
            float weaponDelay = Mathf.Max((1f / data.weapon.frequency) - scaledDelay - scaledRestTime, 0);

            StartShootingAnimation(shootSpeed, data.animTrigger);

            yield return new WaitForSeconds(scaledDelay);
            data.weapon.Shoot(layerMask, shootingOffset, angle);
            yield return new WaitForSeconds(scaledRestTime + weaponDelay);

            _isShooting = false;
        }

        private void StartShootingAnimation(float speed, string animTrigger)
        {
            if (_animator == null || string.IsNullOrEmpty(animTrigger)) return;

            _animator.SetFloat("shootSpeed", speed);
            _animator.SetTrigger(animTrigger);
        }

        public void Restart()
        {
            _canShoot = false;
            if (_restartShootingCoroutine != null) StopCoroutine(_restartShootingCoroutine);
            _restartShootingCoroutine = StartCoroutine(RestartCoroutine());
        }
        private IEnumerator RestartCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            if (_shootingCoroutine != null) StopCoroutine(_shootingCoroutine);
            _isShooting = false;
            _canShoot = true;
        }

        public void CanShoot(bool value)
        {
            _canShoot = value;
        }

        public void ChangeShootingOffset(Vector2 offset)
        {
            shootingOffset.transform.localPosition = new(offset.x, offset.y, 0);
        }
    }
}

/*public virtual IEnumerator Shooting()
        {
            isShooting = true;

            currentAngle += 5f;
            if (weapon != null)
                weapon.Shoot(_layerMask, position, currentAngle);

            *//*var projectile = Instantiate(GameManager.Projectile, transform.position, Quaternion.Euler(0, 0, toTargetAngle));
            projectile.Setup(_layerMask, Quaternion.Euler(0, 0, toTargetAngle) * Vector2.right, projectileSpeed, damage);*/

/*float degree = 0;
int j = 6;
float differenceDegree = 30;

currentAngle += 8;

for (int i = 1; i <= 12; i++)
{
    degree = (differenceDegree * (j - (i - 1))) - (differenceDegree / 2);
    degree += currentAngle;

    Quaternion eulerAngle = Quaternion.Euler(0, 0, degree);
    var projectile = Instantiate(GameManager.Projectile, position, eulerAngle);
    projectile.Setup(_layerMask, eulerAngle * Vector2.right, projectileSpeed, damage);
}*/

/*for (int i = 1; i <= data.shots; i++)
{
    if (isArc)
    {
        if (data.shots % 2 == 0) degree = (data.degree * (j - (i - 1))) - (data.degree / 2);
        else degree = data.degree * (j - (i - 1));
    }
    else if (isParametric && data.shots > 1)
    {
        if (i % 2 == 0) degree = -33.75f + 22.5f * (j - (i - 1)) + ((i <= j) ? 90f : 0);
        else degree = -33.75f + 22.5f * (j - (i - 1)) + ((i <= j) ? 0 : -90f);
    }

    var bullet = Instantiate(AdventureManager.ProjectilePrefab, mousePosition, Quaternion.Euler(0, 0, degree));
    bullet.Setup(projectileMask, data.projectileSprite, (i % 2 == 0) ? 1 : -1, Random.Range(data.minDamage, data.maxDamage),
        Quaternion.AngleAxis(degree, Vector3.forward) * Vector3.right,
        0,
        data.speed,
        data.lifetime,
        data.frequency,
        (i % 2 == 0) ? -data.amplitude : data.amplitude,
        data.magnitude,
        data.shootType[0],
        isParametric ? ProjectileEffect.tracking : ProjectileEffect.none);
}*//*

yield return new WaitForSeconds(1f / weapon.frequency);

isShooting = false;
}*/