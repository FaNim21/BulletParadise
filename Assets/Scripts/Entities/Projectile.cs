using BulletParadise.Shooting;
using BulletParadise.World;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _body;

        public ProjectileBehavior behavior;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float _aliveTimer;


        public void Setup(int layerMask, ProjectileBehavior behavior, Vector2 position, Quaternion rotation)
        {
            if (gameObject.layer != layerMask)
            {
                gameObject.layer = layerMask;
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.layer = layerMask;
            }

            transform.SetPositionAndRotation(position, rotation);
            _rb.rotation = transform.eulerAngles.z;

            _spriteRenderer.sprite = behavior.data.sprite;
            _aliveTimer = 0;

            this.behavior = behavior;
            this.behavior.OnInitialize(_rb, _body);

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (behavior == null) return;

            _aliveTimer += Time.deltaTime;
            if (_aliveTimer >= behavior.data.lifeTime)
            {
                Destroy();
                return;
            }

            behavior.logic.OnUpdate(behavior);
        }
        private void FixedUpdate()
        {
            if (behavior == null) return;
            behavior.physics.OnUpdate(behavior);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.parent.TryGetComponent<IDamageable>(out var entity))
            {
                entity.TakeDamage(behavior.data.damage);
                Destroy();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
            //ProjectilePooler.Instance.Release(this);
        }

        /*public void Restart()
        {
            _aliveTimer = 0f;
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            behavior = null;
        }*/
    }
}
