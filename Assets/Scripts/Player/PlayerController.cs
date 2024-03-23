using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using BulletParadise.Entities;
using BulletParadise.UI;
using BulletParadise.Visual.Drawing;
using BulletParadise.Visual;
using BulletParadise.Constants;
using BulletParadise.Misc;
using BulletParadise.World;
using BulletParadise.UI.Windows;
using System.Collections;
using BulletParadise.Shooting;

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
        public CameraController cameraController;
        public CanvasHandle canvasHandle;
        public LevelLoader levelLoader;

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
        public Weapon weapon;
        public float speed;
        public float sprintSpeed;
        public float projectileSpeed;

        [Header("Debug")]
        [ReadOnly] public bool isInLobby;
        [ReadOnly] public bool isResponding;
        [ReadOnly, SerializeField] private bool isDead;
        [ReadOnly, SerializeField] private bool isInvulnerable;
        [ReadOnly, SerializeField] private bool isShooting;
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

            gameManager.drawDebug.AddGlobalDrawable(this);
            isInLobby = true;
            isResponding = true;

            /*healthValue.SetText(health.ToString());
            maxHealthValue.SetText(maxHealth.ToString());*/
            //healthFill.fillAmount = health / maxHealth;

            animator.SetFloat("moveY", -1);
            animator.SetFloat("shootSpeed", 3f);
        }
        public override void Start()
        {
            base.Start();

            cameraController = CameraController.Instance;
        }
        public override void Update()
        {
            base.Update();

            HandleInput();

            if (!isResponding) return;

            mousePosition = Utils.GetMouseWorldPosition();
            aimDirection = (mousePosition - (Vector2)shootingOffset.position).normalized;
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            animator.SetFloat("aimDirX", aimDirection.x);
            animator.SetFloat("aimDirY", aimDirection.y);

            if (Mouse.current.leftButton.isPressed && !canvasHandle.isCanvasEnabled && !EventSystem.current.IsPointerOverGameObject() && !isShooting) StartCoroutine(Shoot());
        }

        public override void FixedUpdate()
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * inputDirection);
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

            //GLDraw.DrawCircle(shootingOffset.position, gameManager.worldManager.renderingRadius, Color.yellow);

            //kierunek patrzenia
            GLDraw.DrawRay(shootingOffset.position, aimDirection * 1.5f, Color.blue);

            //Kolizja gracza
            GLDraw.DrawBox((Vector2)body.position + boxCollider.offset, boxCollider.size, Color.green);
        }

        private void HandleInput()
        {
            if (Consts.IsFocusedOnUI) return;

            if (isInLobby)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    StopMovement();
                    canvasHandle.CloseUIWindow();
                }
            }
            else
            {
                if (Keyboard.current.rKey.wasPressedThisFrame) ReturnToLobby();
            }

            if (Keyboard.current.f1Key.wasPressedThisFrame) gameManager.drawDebug.SwitchDebugMode();

            if (Consts.IsFocusedOnMainMenu) return;
            //TU BINDY KTORE NIE MOGA DZIALAC NA MAIN MENU
        }

        public void HandleMovement(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && !(context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled) || !isResponding) return;
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

        private IEnumerator Shoot()
        {
            isShooting = true;
            animator.SetTrigger("shoots");

            if (weapon != null)
                weapon.Shoot(_layerMask, shootingOffset.position, aimAngle);

            /*var projectile = Instantiate(GameManager.Projectile, shootingOffset.position, Quaternion.Euler(0, 0, aimAngle));
            projectile.Setup(_layerMask, Quaternion.Euler(0, 0, aimAngle) * Vector2.right, projectileSpeed, 10);*/

            yield return new WaitForSeconds(0.2f);
            isShooting = false;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || isInvulnerable) return;

            if (isDead) return;
            if (health <= 0f) OnDeath();
            health -= damage;

            Popup.Create(position, damage.ToString(), Color.red);
            UpdateHealthBar();
        }
        private void UpdateHealthBar()
        {
            Vector3 barScale = healthBar.localScale;
            barScale.x = Mathf.Clamp01(health / maxHealth);
            healthBar.localScale = barScale;

            //healthFill.fillAmount = health / maxHealth;
            //healthValue.SetText(health.ToString());
        }

        private void OnDeath()
        {
            StopMovement();
            isDead = true;
            isResponding = false;
            boxCollider.enabled = false;
            canvasHandle.OpenWindow<DeathScreen>();
            animator.SetTrigger("died");
            Utils.Log("DEATH");
        }

        public void Restart()
        {
            transform.position = Vector2.zero;
            health = maxHealth;
            isDead = false;
            boxCollider.enabled = true;
            cameraController.transform.position = Vector2.zero;
            UpdateHealthBar();
        }

        public void ReturnToLobby()
        {
            levelLoader.LoadScene("Lobby");
            animator.Rebind();
            canvasHandle.CloseWindow<DeathScreen>();
            isInLobby = true;
        }

        public void StopMovement()
        {
            inputDirection = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
    }
}