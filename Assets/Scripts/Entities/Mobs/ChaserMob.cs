using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise.Entities.Mobs
{
    public class ChaserMob : MobController
    {
        [Header("Chaser Debug")]
        [ReadOnly] public bool isNotRoaming;
        [ReadOnly] public bool isChasing;
        [ReadOnly] public float chaseTimer;
        [ReadOnly] public float roamTimer;
        [ReadOnly] public float toRoamAngle;
        [ReadOnly] public Vector2 toRoamPosition;
        [ReadOnly] public Vector2 toRoamDirection;

        private const float _chaseTime = 1f;
        private const float _roamTime = 3f;


        public override void Awake()
        {
            base.Awake();

            isChasing = true;
            isNotRoaming = true;

            currentSpeed = config.chaseSpeed;
        }

        public override void Update()
        {
            base.Update();

            shootingManager.Shoot(weapon, 0);

            if (isChasing)
            {
                Chase();
                return;
            }

            if (isNotRoaming)
            {
                StartRoam();
                return;
            }

            Roam();
        }
        public override void FixedUpdate()
        {
            rb.MovePosition(rb.position + Avoidance() * 4f + currentSpeed * Time.deltaTime * direction);
        }

        private void Chase()
        {
            chaseTimer += Time.deltaTime;
            direction = playerDirection;

            if (chaseTimer >= _chaseTime)
            {
                isChasing = false;
                chaseTimer = 0f;
            }
        }

        private void StartRoam()
        {
            toRoamPosition = position + (GetRandomNormalizedUnitVector() * Random.Range(config.roamDistance.x, config.roamDistance.y));
            isNotRoaming = false;
            currentSpeed = config.speed;
        }

        private void Roam()
        {
            roamTimer += Time.deltaTime;
            toRoamDirection = (toRoamPosition - position).normalized;
            toRoamAngle = Mathf.Atan2(toRoamDirection.y, toRoamDirection.x) * Mathf.Rad2Deg;
            direction = toRoamDirection;

            if (IsTargetInDistance(toRoamPosition, 1) || roamTimer >= _roamTime)
            {
                isNotRoaming = true;
                isChasing = true;
                currentSpeed = config.chaseSpeed;
                roamTimer = 0f;
            }
        }

        public override void Draw()
        {
            base.Draw();

            GLDraw.DrawLine(position, toRoamPosition, Color.red, 0.01f);
        }
    }
}
