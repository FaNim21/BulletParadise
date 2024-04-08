using BulletParadise.Entities;
using BulletParadise.Visual;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletParadise
{
    public class HealthManager : MonoBehaviour, IDamageable, IHealable
    {
        public Entity entity;

        [Header("Obiekty")]
        public Transform healthBar;

        [Header("UI")]
        public Image healthFill;
        /*public TextMeshProUGUI healthValue;
        public TextMeshProUGUI maxHealthValue;*/

        [Header("Values")]
        public bool useHealthBar;

        [Header("Debug")]
        [ReadOnly, SerializeField] private bool canRegenerate;
        [ReadOnly, SerializeField] private bool isDead;
        [ReadOnly, SerializeField] private bool isInvulnerable;
        [ReadOnly, SerializeField] private bool isSick; //TODO: 5 temp do momentu jak zrobie juz po bosach debuff system
        [ReadOnly, SerializeField] private int vitality = 50;


        private void Update()
        {
            if (!canRegenerate || isSick) return;

            //TODO: 0 regenerowanie hp
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || isInvulnerable) return;

            if (isDead) return;
            if (entity.health <= 0f) OnDeath();
            entity.health -= damage;

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
            Vector3 barScale = healthBar.localScale;
            barScale.x = entity.GetHealthToMaxProportion();
            healthBar.localScale = barScale;

            //healthFill.fillAmount = health / maxHealth;
            //healthValue.SetText(health.ToString());
        }

        private void OnDeath()
        {
            isDead = true;
            entity.OnDeath();
        }

        public void Restart()
        {
            entity.health = entity.maxHealth;
            isDead = false;
            UpdateHealthBar();
        }
    }
}