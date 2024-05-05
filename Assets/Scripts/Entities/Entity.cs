using BulletParadise.Components;
using BulletParadise.World;
using UnityEngine;

namespace BulletParadise.Entities
{
    [RequireComponent(typeof(HealthManager))]
    public abstract class Entity : MonoBehaviour, IDrawable
    {
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public HealthManager healthManager;
        [HideInInspector] public new Transform transform;

        [HideInInspector] public Vector2 position;
        [HideInInspector] public float health;
        [HideInInspector] public int maxHealth;

        //TODO: 2 to bedzie zalezne od world managera dla ENTITY
        public bool CanDraw => true;


        public virtual void Awake()
        {
            transform = GetComponent<Transform>();
            healthManager = GetComponent<HealthManager>();
            rb = GetComponent<Rigidbody2D>();

            health = maxHealth;
        }
        public virtual void Start() { }
        public virtual void Update()
        {
            position = transform.position;
        }
        public abstract void FixedUpdate();

        public abstract void Draw();

        public virtual void OnHit() { }
        public virtual void OnDeath() { }

        public float GetHealthToMaxProportion() => Mathf.Clamp01(health / maxHealth);

        public Vector2 GetRandomNormalizedUnitVector()
        {
            float random = Random.Range(0, 2 * Mathf.PI);

            return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        }
    }
}