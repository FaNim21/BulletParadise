using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform body;

        public ProjectileBehavior behavior;


        public void Setup(string layerMask, ProjectileBehavior behavior)
        {
            gameObject.layer = LayerMask.NameToLayer(layerMask);
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(layerMask);
            spriteRenderer.sprite = behavior.data.sprite;

            this.behavior = behavior;
            this.behavior.OnInitialize(rb, body);

            Destroy(gameObject, behavior.data.lifeTime);
        }

        private void Update() => behavior.logic.OnUpdate(behavior);
        private void FixedUpdate() => behavior.physics.OnUpdate(behavior);

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
