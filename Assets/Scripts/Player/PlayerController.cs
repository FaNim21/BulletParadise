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
using System.Collections.Generic;

namespace BulletParadise.Player
{
    public sealed class PlayerController : Entity, IDamageable
    {
        public static PlayerController Instance { get; private set; }

        [Header("Globals")]
        public GameManager gameManager;

        [Header("Components")]
        public QuickBar quickBar;
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
        public float speed;

        [Header("Debug")]
        public bool autoFire;
        [ReadOnly] public bool isInLobby;
        [ReadOnly] public bool isResponding;
        [ReadOnly, SerializeField] private bool isDead;
        [ReadOnly, SerializeField] private bool isInvulnerable;
        [ReadOnly, SerializeField] private bool isShooting;
        [ReadOnly, SerializeField] private bool canShoot;
        [ReadOnly] public float aimAngle;
        [ReadOnly] public Vector2 inputDirection;
        [ReadOnly] public Vector2 mousePosition;
        [ReadOnly] public Vector2 aimDirection;

        private readonly List<IInteractable> interactables = new();
        private readonly string _layerMask = "ProjectilePlayer";

        private Coroutine _shootingCoroutine;
        private Coroutine _restartShootingCoroutine;


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

            HandleShooting();
        }

        public override void FixedUpdate()
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * inputDirection);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                foreach (var interaction in interactables)
                    interaction.LostFocus();

                interactable.Focus();
                interactables.Add(interactable);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                interactable.LostFocus();
                interactables.Remove(interactable);
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
        private void HandleShooting()
        {
            if (!canShoot) return;

            if (autoFire)
            {
                if (!isShooting)
                    _shootingCoroutine = StartCoroutine(Shoot());
            }
            else
            {
                if (Mouse.current.leftButton.isPressed && !canvasHandle.isCanvasEnabled && !EventSystem.current.IsPointerOverGameObject() && !isShooting)
                    _shootingCoroutine = StartCoroutine(Shoot());
            }
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed || interactables == null || interactables.Count == 0) return;

            interactables[^1].Interact();
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
            if (quickBar.weapon == null) yield break;

            isShooting = true;
            animator.SetTrigger("shoots");

            quickBar.weapon.Shoot(_layerMask, shootingOffset.position, aimAngle);

            yield return new WaitForSeconds(1f / quickBar.weapon.frequency);
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

        public void SetResponding(bool option)
        {
            isResponding = option;
            if (!option)
            {
                StopMovement();
            }
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

        public void SetWeapon(Weapon weapon)
        {
            quickBar.SetWeaponToSlot(weapon);
            RestartShooting();
        }

        public void RestartShooting()
        {
            canShoot = false;
            if (_restartShootingCoroutine != null) StopCoroutine(_restartShootingCoroutine);
            _restartShootingCoroutine = StartCoroutine(RestartShootingCoroutine());
        }
        private IEnumerator RestartShootingCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            if (_shootingCoroutine != null) StopCoroutine(_shootingCoroutine);
            isShooting = false;
            canShoot = true;
        }
    }
}