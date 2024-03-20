using BulletParadise.Misc;
using BulletParadise.Player;
using BulletParadise.Visual;
using BulletParadise.Visual.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class MobController : Entity, IDamageable
    {
        public static List<MobController> mobs = new();

        [Header("Komponenty")]
        public Rigidbody2D rb;
        public BoxCollider2D boxCollider;

        [Header("Obiekty")]
        public Transform target;
        public Transform body;
        public Transform healthBar;

        [Header("Wartosci")]
        public float chaseRange;
        public float moveSpeed;

        public int damage;
        public float projectileSpeed;
        public float shootingCooldown;
        public int exp;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool isInvulnerable;
        [SerializeField, ReadOnly] private bool isShooting;
        [SerializeField, ReadOnly] private Vector2 direction;
        [SerializeField, ReadOnly] private float toTargetAngle;
        [SerializeField, ReadOnly] private float currentAngle;

        private readonly string _layerMask = "ProjectileMob";

        public override void Awake()
        {
            base.Awake();

            mobs.Add(this);
        }
        public override void Start()
        {
            //GameManager.AddDrawable(this);
            target = PlayerController.Instance.transform;
            //isInvulnerable = true;
        }

        public override void Update()
        {
            base.Update();

            direction = (target.position - transform.position).normalized;
            toTargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (IsTargetInDistance(chaseRange) && !isShooting) StartCoroutine(Shooting());

            if (health <= 0)
            {
                mobs.Remove(this);
                //PlayerController.Instance.levelSystem.AddExp(exp);
                Destroy(gameObject);
            }
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

        public void TakeDamage(int damage)
        {
            //Narazie jest to bazowa metoda do przyjmowania dmg
            if (isInvulnerable) return;

            Popup.Create(transform.position, damage.ToString(), Color.red);
            health -= damage;

            Vector3 barScale = healthBar.localScale;
            barScale.x = health / maxHealth;
            healthBar.localScale = barScale;
        }
        public virtual IEnumerator Shooting()
        {
            isShooting = true;

            /*var projectile = Instantiate(GameManager.Projectile, transform.position, Quaternion.Euler(0, 0, toTargetAngle));
            projectile.Setup(_layerMask, Quaternion.Euler(0, 0, toTargetAngle) * Vector2.right, projectileSpeed, damage);*/

            float degree = 0;
            int j = 6;
            float differenceDegree = 30;

            currentAngle += 8;

            for (int i = 1; i <= 12; i++)
            {
                degree = (differenceDegree * (j - (i - 1))) - (differenceDegree / 2);
                degree += currentAngle;

                Quaternion eulerAngle = Quaternion.Euler(0, 0, degree);
                var projectile = Instantiate(GameManager.Projectile, transform.position, eulerAngle);
                projectile.Setup(_layerMask, eulerAngle * Vector2.right, projectileSpeed, damage);
            }

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
            }*/

            yield return new WaitForSeconds(shootingCooldown);

            isShooting = false;
        }

        public bool IsTargetInDistance(float distance)
        {
            float sqrDistance = (target.position - transform.position).sqrMagnitude;
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

            Utils.Log(avoidVector);
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

                if (distance > range)
                    continue;

                if (closestMob == null)
                    closestMob = currentMob;
                else if (Vector2.Distance(position, currentMob.position) < Vector2.Distance(position, closestMob.position))
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