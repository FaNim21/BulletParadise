using BulletParadise.Misc;
using BulletParadise.Shooting;
using System.Collections;
using UnityEngine;

namespace BulletParadise.Components
{
    public class ShootingManager : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private Transform shootingOffset;

        public int layerMask;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isShooting;
        [SerializeField, ReadOnly] private bool _canShoot;

        private Coroutine _shootingCoroutine;
        private Coroutine _restartShootingCoroutine;


        private void Awake()
        {
            _animator = transform.Find("Body").GetComponent<Animator>();
        }

        public void Shoot(Weapon weapon, float angle)
        {
            if (_isShooting || weapon == null || !_canShoot) return;

            StartCoroutine(ShootCoroutine(weapon, angle));
        }
        private IEnumerator ShootCoroutine(Weapon weapon, float angle)
        {
            _isShooting = true;
            if (_animator != null) _animator.SetTrigger("shoots");

            weapon.Shoot(layerMask, shootingOffset.position, angle);

            yield return new WaitForSeconds(1f / weapon.frequency);
            _isShooting = false;
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
    }
}