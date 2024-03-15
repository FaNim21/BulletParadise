using UnityEngine;

namespace BulletParadise.Entities
{
    public class Projectile : MonoBehaviour
    {
        public Rigidbody2D rb;

        public int damage;
        public float speed;

        private Vector2 velocity;

        public void Setup(string layerMask, Vector2 velocity, float speed, int damage)
        {
            gameObject.layer = LayerMask.NameToLayer(layerMask);
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(layerMask);

            this.velocity = velocity;
            this.speed = speed;
            this.damage = damage;

            Destroy(gameObject, 3f);
        }

        private void FixedUpdate()
        {
            //TODO: 0 tu zrobic system, ktory mialem w glowie odnosnie interfejsu do tego zeby robic DI w setupie dla patternow
            rb.MovePosition(rb.position + speed * Time.deltaTime * velocity);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.parent.TryGetComponent<Entity>(out var entity))
            {
                entity.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
