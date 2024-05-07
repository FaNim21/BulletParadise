using BulletParadise.Shooting;
using BulletParadise.Visual.Drawing;
using UnityEngine;
using UnityEngine.XR;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "new RoamFromMiddlePhase", menuName = "Boss/Phase/RoamFromMiddle")]
    public class RoamFromMiddleAreaPhase : Phase
    {
        public float speed;
        public float roamRange = 2;
        public float timeOnDestination = 1f;

        public Weapon primaryWeapon;
        public string primaryWeaponAnimName;
        public float primaryWeaponAnimTime;
        public float primaryWeaponAnimDelay;

        private float _currentSpeed;
        private bool _reachedDestination;
        private bool _shoot;
        private float _timer;
        private Vector2 _arenaCenter;
        private Vector2 _toRoamDirection;
        private Vector2 _toRoamPosition;

        private const float _distanceThreshold = 0.25f;


        public override void OnEnter()
        {
            _timer = 0f;
            _arenaCenter = boss.arenaCenter;
            _reachedDestination = false;
            _shoot = true;
            _currentSpeed = speed;
            boss.SetSpeedAnim(_currentSpeed);

            _toRoamPosition = _arenaCenter + boss.GetRandomNormalizedUnitVector() * roamRange;
        }

        public override void LogicUpdate(Weapon weapon, Vector2 targetDirection)
        {
            if (_reachedDestination)
            {
                _timer += Time.deltaTime;

                if (_timer >= timeOnDestination)
                {
                    _timer = 0f;
                    _reachedDestination = false;
                    _toRoamPosition = _arenaCenter + boss.GetRandomNormalizedUnitVector() * roamRange;
                    _currentSpeed = speed;
                    boss.SetSpeedAnim(_currentSpeed);
                    if (primaryWeapon != null)
                    {
                        shootingManager.Shoot(primaryWeapon, 0, null, primaryWeaponAnimName, primaryWeaponAnimDelay, primaryWeaponAnimTime);
                    }
                    return;
                }

                if (primaryWeapon != null && _shoot)
                {
                    shootingManager.Shoot(primaryWeapon, 0, null, primaryWeaponAnimName, primaryWeaponAnimDelay, primaryWeaponAnimTime);
                    _shoot = false;
                }
                return;
            }

            _toRoamDirection = (_toRoamPosition - (Vector2)boss.transform.position).normalized;

            if (boss.IsTargetInDistance(_toRoamPosition, _distanceThreshold))
            {
                _reachedDestination = true;
                _currentSpeed = 0;
                boss.SetSpeedAnim(0);
                _shoot = true;
            }
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            rb.MovePosition(rb.position + _currentSpeed * Time.deltaTime * _toRoamDirection);
        }

        public override void OnExit()
        {

        }

        public override void Draw()
        {
            GLDraw.DrawCircle(_arenaCenter, 2 * Mathf.PI * roamRange, Color.green);
            GLDraw.DrawLine(boss.transform.position, _toRoamPosition, Color.yellow);
        }
    }
}
