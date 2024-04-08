using BulletParadise.Player;
using BulletParadise.Shooting;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletParadise.UI
{
    public class WeaponInformationWindow : MonoBehaviour
    {
        [Header("Componenets")]
        public GameObject background;
        public TextMeshProUGUI titleText;
        public Image image;
        public TextMeshProUGUI mainInfoText;

        [Header("Values")]
        public Weapon weapon;

        private StringBuilder _mainInfoSb = new();


        public void Setup(Weapon weapon)
        {
            this.weapon = weapon;
            titleText.SetText(weapon.name);
            image.sprite = weapon.sprite;

            _mainInfoSb.Clear();
            _mainInfoSb.AppendLine($"Shots: {weapon.projectiles.Length}");

            float avgDamage = 0f;
            float sumDamage;
            for (int i = 0; i < weapon.projectiles.Length; i++)
            {
                var current = weapon.projectiles[i];
                avgDamage += current.data.damage;
            }
            sumDamage = avgDamage;
            avgDamage /= weapon.projectiles.Length;

            _mainInfoSb.AppendLine($"Avg Damage: {Mathf.Round(avgDamage * 100f) / 100f}");
            _mainInfoSb.AppendLine($"Sum Damage: {sumDamage}");
            _mainInfoSb.AppendLine($"DPS: {sumDamage / (1f / weapon.frequency)}");
            _mainInfoSb.AppendLine($"Frequency: {weapon.frequency} per sec");
            mainInfoText.SetText(_mainInfoSb);

            background.SetActive(true);
        }

        public void Exit()
        {
            background.SetActive(false);
        }
    }
}