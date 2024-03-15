using BulletParadise.World;
using UnityEngine;

namespace BulletParadise.Entities
{
    public abstract class Entity : MonoBehaviour, IDrawable
    {
        [HideInInspector] public new Transform transform;

        [ReadOnly] public Vector2 position;
        [ReadOnly] public float health;
        public int maxHealth;

        //TODO: 2 to bedzie zalezne od world managera dla ENTITY
        public bool CanDraw => true;


        public virtual void Awake()
        {
            transform = GetComponent<Transform>();
            health = maxHealth;
        }
        public virtual void Start()
        {
            //TODO: 0 to mozlwie ze w awake zeby zrobic tez dodawania w world managerze rzeczy do draw debug
            //GameManager.Instance.worldManager.AddEntityToWorld(this);
        }
        public virtual void Update()
        {
            position = transform.position;
        }
        public abstract void FixedUpdate();
        public abstract void TakeDamage(int damage);

        public abstract void Draw();
    }
}