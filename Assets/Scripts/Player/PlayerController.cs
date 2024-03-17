using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using BulletParadise.Entities;
using BulletParadise.UI;
using BulletParadise.Visual.Drawing;
using BulletParadise.Visual;
using BulletParadise.Constants;
using BulletParadise.Misc;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
/*using GamePlay.Phone;
using Engine.Equipment;
using Engine.UI.Console;
using Player.Inputs;*/

namespace BulletParadise.Player
{
    public sealed class PlayerController : Entity, IDamageable
    {
        public static PlayerController Instance { get; private set; }

        [Header("Globals")]
        public GameManager gameManager;

        [Header("Komponenty")]
        //public InputsHandler inputHandler;
        //public Inventory inventory;
        public CanvasHandle canvasHandle;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;
        [SerializeField] private BoxCollider2D boxCollider;

        [Header("Obiekty")]
        public Transform body;
        public Transform shootingOffset;
        public Transform healthBar;

        [Header("UI")]
        public Image healthFill;
        /*public TextMeshProUGUI healthValue;
        public TextMeshProUGUI maxHealthValue;*/

        [Header("G³ówne wartoœci")]
        public float speed;
        public float sprintSpeed;
        public float projectileSpeed;

        [Header("Debug")]
        [ReadOnly] public bool isInvulnerable;
        [ReadOnly] public float currentSpeed;
        [ReadOnly] public float aimAngle;
        [ReadOnly] public Vector2 inputDirection;
        [ReadOnly] public Vector2 mousePosition;
        [ReadOnly] public Vector2 aimDirection;

        private readonly string _layerMask = "ProjectilePlayer";


        public override void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            base.Awake();

            gameManager.AddDrawableDebug(this);
            //gameManager.worldManager.enabled = true; //TODO 9999 WTF to musi tu byc poniewaz po buildzie worldmanager script enabled jest ustawione na false?????? unity moment

            /*healthValue.SetText(health.ToString());
            maxHealthValue.SetText(maxHealth.ToString());*/
            //healthFill.fillAmount = health / maxHealth;

            currentSpeed = speed;
            animator.SetFloat("moveX", 1);
        }
        public override void Update()
        {
            base.Update();

            HandleInput();

            mousePosition = Utils.GetMouseWorldPosition();
            aimDirection = (mousePosition - (Vector2)shootingOffset.position).normalized;
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (Mouse.current.leftButton.wasPressedThisFrame && !canvasHandle.isCanvasEnabled && !EventSystem.current.IsPointerOverGameObject()) Shoot();
        }

        public override void FixedUpdate()
        {
            rb.MovePosition(rb.position + currentSpeed * Time.deltaTime * inputDirection);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IEnterable portal))
            {
                canvasHandle.enterWindow.Setup(portal.GetSceneName());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IEnterable _))
            {
                canvasHandle.enterWindow.Exit();
            }
        }

        public override void Draw()
        {
            if (transform == null) return;

            GLDraw.DrawCircle(shootingOffset.position, gameManager.worldManager.renderingRadius, Color.yellow);
            GLDraw.DrawCircle(shootingOffset.position, gameManager.worldManager.detectBagsRadius, Color.cyan);

            //kierunek patrzenia
            GLDraw.DrawRay(shootingOffset.position, aimDirection * 1.5f, Color.blue);

            //Kolizja gracza
            GLDraw.DrawBox((Vector2)body.position + boxCollider.offset, boxCollider.size, Color.green);
        }

        private void HandleInput()
        {
            if (Consts.IsFocusedOnUI) return;
            if (Keyboard.current.escapeKey.wasPressedThisFrame) canvasHandle.CloseUIWindow();
            if (Consts.IsFocusedOnMainMenu) return;
            if (Keyboard.current.rKey.wasPressedThisFrame) SceneManager.LoadScene(0);
        }

        public void HandleMovement(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && !(context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)) return;
            if (Consts.IsFocusedOnUI || Consts.IsFocusedOnMainMenu) return;

            Vector2 _input = context.ReadValue<Vector2>();
            inputDirection.x = _input.x;
            inputDirection.y = _input.y;
            inputDirection = inputDirection.normalized;

            animator.SetFloat("Speed", inputDirection.magnitude);

            if (context.phase == InputActionPhase.Performed)
            {

                if (context.phase == InputActionPhase.Canceled) return;

                if (canvasHandle.isCanvasEnabled) return;

                if (inputDirection != Vector2.zero)
                {
                    animator.SetFloat("moveX", inputDirection.x);
                    animator.SetFloat("moveY", inputDirection.y);
                }
            }
        }

        private void Shoot()
        {
            var projectile = Instantiate(GameManager.Projectile, shootingOffset.position, Quaternion.Euler(0, 0, aimAngle));
            projectile.Setup(_layerMask, Quaternion.Euler(0, 0, aimAngle) * Vector2.right, projectileSpeed, 10);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || isInvulnerable) return;

            health -= damage;

            Popup.Create(transform.position, damage.ToString(), Color.red);
            UpdateHealthBar();

            if (health <= 0f) Respawn();
        }
        private void UpdateHealthBar()
        {
            Vector3 barScale = healthBar.localScale;
            barScale.x = health / maxHealth;
            healthBar.localScale = barScale;

            //healthFill.fillAmount = health / maxHealth;
            //healthValue.SetText(health.ToString());
        }

        public void Respawn()
        {
            health = maxHealth;
        }

        public void StopMovement()
        {
            inputDirection = Vector2.zero;
        }
    }
}