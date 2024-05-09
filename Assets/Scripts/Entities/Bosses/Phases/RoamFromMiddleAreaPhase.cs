using BulletParadise.Components;
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

        public WeaponShootBossData weaponShootingData;

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

                    shootingManager.Shoot(weaponShootingData, 0);
                    return;
                }

                if (_shoot)
                {
                    shootingManager.Shoot(weaponShootingData, 0);
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
