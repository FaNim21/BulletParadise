using BulletParadise.World;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletParadise.UI.Windows
{
    public class DebugScreen : MonoBehaviour
    {
        private GameObject background;

        public TextMeshProUGUI allProjectilesText;
        public TextMeshProUGUI activeProjectilesText;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int _allProjectiles;
        [SerializeField, ReadOnly] private int _activeProjectiles;


        private void Awake()
        {
            background = transform.GetChild(0).gameObject;

            activeProjectilesText.SetText("Active projectiles: 0");
        }
        private void Start()
        {
            background.SetActive(false);
        }

        private void Update()
        {
            if (!background.activeSelf) return;

            /*int allProjectiles = ProjectilePooler.Instance.GetProjectilesInPool();
            if (allProjectiles != _allProjectiles)
            {
                _allProjectiles = allProjectiles;
                allProjectilesText.SetText("All projectiles: " + _allProjectiles.ToString());
            }

            int activeProjectiles = ProjectilePooler.Instance.GetActiveProjectilesInPool();
            if (activeProjectiles != _activeProjectiles)
            {
                _activeProjectiles = activeProjectiles;
                activeProjectilesText.SetText("Active projectiles: " + _activeProjectiles.ToString());
            }*/
        }

        public void SwitchScreenVisibility(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Canceled) return;

            background.SetActive(!background.activeSelf);
        }
        public void SwitchLinesVisibility(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Canceled) return;

            GameManager.Instance.drawDebug.SwitchDebugMode();
        }
    }
}
