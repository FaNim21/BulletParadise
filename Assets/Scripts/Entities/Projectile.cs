using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public Rigidbody2D rb;
        public Transform body;

        public ProjectileBehavior behavior;

        /*public int damage;
        public float speed;*/
        //public float rotationSpeed;

        //private Vector2 velocity;

        public void Setup(string layerMask, ProjectileBehavior behavior)
        {
            gameObject.layer = LayerMask.NameToLayer(layerMask);
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(layerMask);

            this.behavior = behavior;
            spriteRenderer.sprite = behavior.data.sprite;

            /*this.velocity = velocity;
            this.speed = speed;
            this.damage = damage;*/

            Destroy(gameObject, behavior.data.lifeTime);
        }

        private void Update()
        {
            /*Vector3 childAngle = body.eulerAngles;
            childAngle.z += rotationSpeed * Time.deltaTime;
            body.eulerAngles = childAngle;*/
            behavior.UpdateLogic();
        }

        private void FixedUpdate()
        {
            //TODO: 0 tu zrobic system, ktory mialem w glowie odnosnie interfejsu do tego zeby robic DI w setupie dla patternow
            //rb.MovePosition(rb.position + speed * Time.deltaTime * velocity);
            behavior.UpdatePhysics();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.parent.TryGetComponent<IDamageable>(out var entity))
            {
                entity.TakeDamage(behavior.data.damage);
                Destroy(gameObject);
            }
        }
    }
}
