using BulletParadise.Components;
using BulletParadise.Player;
using BulletParadise.Shooting;
using BulletParadise.Visual.Drawing;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class MobController : Entity
    {
        public static List<MobController> mobs = new();

        [Header("Komponenty")]
        public BoxCollider2D boxCollider;
        public ShootingManager shootingManager;

        [Header("Obiekty")]
        public Transform target;
        public Transform body;

        [Header("Wartosci")]
        public Weapon weapon;
        public float chaseRange;
        public float moveSpeed;

        public int exp;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector2 direction;
        [SerializeField, ReadOnly] private float toTargetAngle;
        [SerializeField, ReadOnly] private float currentAngle;


        public override void Awake()
        {
            base.Awake();

            shootingManager = GetComponent<ShootingManager>();

            mobs.Add(this);
        }
        public override void Start()
        {
            target = PlayerController.Instance.transform;
            GameManager.AddDrawable(this);
        }
        public void OnDestroy()
        {
            GameManager.RemoveDrawable(this);
        }

        public override void Update()
        {
            base.Update();

            direction = ((Vector2)target.position - position).normalized;
            toTargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (IsTargetInDistance(chaseRange)) shootingManager.Shoot(weapon, toTargetAngle);
        }
        public override void FixedUpdate()
        {
            if (IsTargetInDistance(chaseRange) && !IsTargetInDistance(1f))
                rb.MovePosition(rb.position + Separation() * 2f + moveSpeed * Time.deltaTime * direction);
        }

        public override void Draw()
        {
            if (transform == null) return;

            GLDraw.DrawRay(position, direction * 3, Color.cyan);
            GLDraw.DrawCircle(position, chaseRange, Color.red);

            GLDraw.DrawBox((Vector2)body.position + boxCollider.offset, boxCollider.size, Color.green, 0.01f);
        }

        public override void OnDeath()
        {
            mobs.Remove(this);
            Destroy(gameObject);
        }

        public bool IsTargetInDistance(float distance)
        {
            float sqrDistance = ((Vector2)target.position - position).sqrMagnitude;
            return sqrDistance < distance * distance;
        }

        /// <summary>
        /// Unikanie mobów od siebie metod¹ ucieczki od siebie
        /// </summary>
        public Vector2 Avoidance()
        {
            Vector2 avoidVector = Vector2.zero;

            float avoidanceRadius = 0.75f;
            var mobList = GetMobsInRange(position, avoidanceRadius);

            if (mobList.Count == 0 || mobList == null) return avoidVector;

            foreach (var mob in mobList)
                avoidVector += RunAway(mob.position);

            return avoidVector.normalized * Time.deltaTime;
        }

        /// <summary>
        /// Oddzielanie mobów od siebie tworz¹c dystans pomiêdzy
        /// </summary>
        public Vector2 Separation()
        {
            Vector2 separateVector = Vector2.zero;

            float separateRadius = 2f;
            var mobList = GetMobsInRange(position, separateRadius);

            if (mobList.Count == 0 || mobList == null) return separateVector;

            foreach (var mob in mobList)
            {
                Vector2 movingTowards = position - mob.position;
                if (movingTowards.magnitude > 0)
                    separateVector += movingTowards.normalized / movingTowards.magnitude;
            }
            return separateVector.normalized * Time.deltaTime;
        }

        public Vector2 RunAway(Vector2 target)
        {
            Vector2 neededDirection = (position - target).normalized;
            return neededDirection;
        }

        public static Entity GetClosestMob(Vector2 position, float range)
        {
            if (mobs.Count == 0) return null;

            Entity closestMob = null;

            for (int i = 0; i < mobs.Count; i++)
            {
                var currentMob = mobs[i];
                float distance = Vector2.Distance(position, currentMob.position);

                if (distance > range) continue;

                if (closestMob == null || distance < Vector2.Distance(position, closestMob.position))
                    closestMob = currentMob;
            }
            return closestMob;
        }
        public static List<Entity> GetMobsInRange(Vector2 position, float range)
        {
            if (mobs.Count == 0) return null;

            List<Entity> inRangeMobs = new();

            for (int i = 0; i < mobs.Count; i++)
            {
                var currentMob = mobs[i];

                float sqrDistance = (currentMob.position - position).sqrMagnitude;
                if (sqrDistance <= range * range)
                    inRangeMobs.Add(currentMob);
            }

            //TO ROBI GARBAGE COLLECTION ZMIENIC NA TOTALNIE COS INNEGO
            //to tez zalezy gdzie bedzie to uzywane bo musialo by to wspolgrac z klasa w ktorej jest czyli tylko w przypadku mobControllera to zadziala zeby tu dac liste mobow w zasiegu i do niej to dodawac zamiast wyrzucac tablice z metody
            return inRangeMobs;
        }
        public static float GetDistanceOfClosestMob(Vector2 position)
        {
            return Vector2.Distance(position, GetClosestMob(position, 1000f).position);
        }
    }
}