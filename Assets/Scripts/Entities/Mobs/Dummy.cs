using System.Collections;
using UnityEngine;

namespace BulletParadise.Entities.Mobs
{
    public class Dummy : MobController
    {
        [Header("Dummy")]
        public Transform timerBar;
        [SerializeField, ReadOnly] private float healTimer;
        [SerializeField, ReadOnly] private bool isShaking;
        [SerializeField] private float healAmount = 500000f;

        private const float healTrigger = 5f;

        public override void Awake()
        {
            base.Awake();
        }

        public override void Update()
        {
            base.Update();

            if (health >= maxHealth || healthManager.IsInvulnerable()) return;
            healTimer += Time.deltaTime;

            if (healTimer >= healTrigger)
            {
                StartCoroutine(HealAndReset());
            }

            UpdateTimerBar();
        }

        public override void OnHit()
        {
            if (isShaking) return;

            StartCoroutine(ShakeDummy());
        }

        private void UpdateTimerBar()
        {
            Vector3 barScale = timerBar.localScale;
            barScale.x = 1 - Mathf.Clamp01(healTimer / healTrigger);
            timerBar.localScale = barScale;
        }

        private IEnumerator ShakeDummy()
        {
            isShaking = true;
            float timer = 0f;
            float duration = 0.15f;
            Quaternion start = body.rotation;
            Quaternion end = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));

            while (timer < duration)
            {
                Quaternion lerpedRotation = Quaternion.Slerp(start, end, timer / duration);
                body.rotation = lerpedRotation;
                timer += Time.deltaTime;
                yield return null;
            }

            body.rotation = end;
            isShaking = false;
        }

        private IEnumerator HealAndReset()
        {
            healthManager.SetInvulnerability(true);
            yield return new WaitForSeconds(0.25f);
            healthManager.Heal((int)healAmount);
            yield return new WaitForSeconds(0.25f);
            healTimer = 0f;
            UpdateTimerBar();
            healthManager.SetInvulnerability(false);
        }
    }
}
