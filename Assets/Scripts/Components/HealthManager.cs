using BulletParadise.Entities;
using BulletParadise.Visual;
using UnityEngine;

namespace BulletParadise.Components
{
    public class HealthManager : MonoBehaviour, IDamageable
    {
        private Entity entity;

        [Header("Obiekty")]
        [SerializeField] private HealthBar mainHealthBar;
        [SerializeField] private Transform healthBar;
        [SerializeField] private GameObject invulnerabilityIcon;

        [Header("Debug")]
        [ReadOnly, SerializeField] private bool isInvulnerable;
        [ReadOnly, SerializeField] private bool canRegenerate;
        [ReadOnly, SerializeField] private bool isDead;
        [ReadOnly, SerializeField] private bool isSick; //TODO: 5 temp do momentu jak zrobie juz po bosach debuff system


        private void Awake()
        {
            entity = GetComponent<Entity>();
        }
        public void Initialize()
        {
            if (mainHealthBar == null) return;
            mainHealthBar.Initialize();
        }

        private void Update()
        {
            if (!canRegenerate || isSick) return;

            //TODO: 4 regenerowanie hp?
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || isInvulnerable || isDead) return;

            entity.health -= damage;
            entity.OnHit();
            if (entity.health <= 0f) OnDeath();

            Popup.Create(entity.position, damage.ToString(), Color.red);
            UpdateHealthBar();
        }

        public bool Heal(int health)
        {
            if (health <= 0 || isSick || entity.health >= entity.maxHealth) return false;

            int needed = (int)(entity.health + health) - entity.maxHealth;
            if (needed > 0) health -= needed;
            entity.health += health;

            Popup.Create(entity.position, health.ToString(), Color.green);
            UpdateHealthBar();
            return true;
        }

        private void UpdateHealthBar()
        {
            float healthPercent = entity.GetHealthToMaxProportion();

            Vector3 barScale = healthBar.localScale;
            barScale.x = healthPercent;
            healthBar.localScale = barScale;

            if (mainHealthBar == null) return;
            mainHealthBar.UpdateHealthBar(healthPercent);
        }

        private void OnDeath()
        {
            isDead = true;
            entity.health = 0f;
            entity.OnDeath();
        }

        public void Restart()
        {
            entity.health = entity.maxHealth;
            isDead = false;
            UpdateHealthBar();
        }

        public void SetInvulnerability(bool value)
        {
            isInvulnerable = value;
            if (invulnerabilityIcon != null) invulnerabilityIcon.SetActive(value);
        }
        public bool IsInvulnerable() => isInvulnerable;
    }
}