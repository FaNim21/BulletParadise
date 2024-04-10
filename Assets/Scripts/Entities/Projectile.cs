using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D rb;
        public Transform body;

        public ProjectileBehavior behavior;

        //public float rotationSpeed;

        public void Setup(string layerMask, ProjectileBehavior behavior)
        {
            gameObject.layer = LayerMask.NameToLayer(layerMask);
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(layerMask);
            spriteRenderer.sprite = behavior.data.sprite;

            this.behavior = behavior;
            this.behavior.OnInitialize(rb);

            Destroy(gameObject, behavior.data.lifeTime);
        }

        /*Vector3 childAngle = body.eulerAngles;
        childAngle.z += rotationSpeed * Time.deltaTime;
        body.eulerAngles = childAngle;*/

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
