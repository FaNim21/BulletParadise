using BulletParadise.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BulletParadise.Components
{
    public class HealthBar : MonoBehaviour
    {
        public Entity entity;

        [Header("Components")]
        public Image bar;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI maxHealthText;

        [Header("Events")]
        public UnityEvent<float> OnUpdateHealthBar;


        private void OnDestroy()
        {
            if (OnUpdateHealthBar == null) return;
            OnUpdateHealthBar.RemoveAllListeners();
        }

        public void Initialize()
        {
            healthText.SetText(entity.health.ToString());
            maxHealthText.SetText(entity.maxHealth.ToString());
        }

        public void UpdateHealthBar(float healthPercent)
        {
            bar.fillAmount = healthPercent;
            healthText.SetText(entity.health.ToString());

            OnUpdateHealthBar?.Invoke(healthPercent);
        }
    }
}