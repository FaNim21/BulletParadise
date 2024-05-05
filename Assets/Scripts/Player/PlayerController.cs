using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using BulletParadise.Entities;
using BulletParadise.UI;
using BulletParadise.Visual.Drawing;
using BulletParadise.Constants;
using BulletParadise.Misc;
using BulletParadise.World;
using BulletParadise.UI.Windows;
using BulletParadise.Shooting;
using System.Collections.Generic;
using BulletParadise.Components;

namespace BulletParadise.Player
{
    public sealed class PlayerController : Entity
    {
        public static PlayerController Instance { get; private set; }

        [Header("Globals")]
        public GameManager gameManager;

        [Header("Components")]
        public PlayerConfig config;
        public QuickBar quickBar;
        public CanvasHandle canvasHandle;
        public LevelLoader levelLoader;
        public ShootingManager shootingManager;
        public DebugScreen debugScreen;

        private Animator _animator;
        private CircleCollider2D _circleCollider;
        private CameraController _cameraController;

        private Transform _body;
        private Transform _shootingOffset;

        [Header("Debug")]
        [ReadOnly] public bool autoFire;
        [ReadOnly] public bool isInLobby;
        [ReadOnly] public bool isResponding;
        [ReadOnly] public float aimAngle;
        [ReadOnly] public Vector2 inputDirection;
        [ReadOnly] public Vector2 mousePosition;
        [ReadOnly] public Vector2 aimDirection;

        private readonly List<IInteractable> _interactables = new();


        public override void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            base.Awake();

            _body = transform.Find("Body");
            _shootingOffset = transform.Find("ShootingOffset");

            _circleCollider = _body.GetComponent<CircleCollider2D>();
            _animator = _body.GetComponent<Animator>();

            gameManager.drawDebug.AddGlobalDrawable(this);
            isInLobby = true;
            isResponding = true;

            _animator.SetFloat("moveY", -1);

            maxHealth = config.maxHealth;

        }
        public override void Start()
        {
            base.Start();

            _cameraController = CameraController.Instance;
            MobController.mobs.Add(this);
            healthManager.Initialize();
        }
        public void OnDestroy()
        {
            gameManager.drawDebug.RemoveGlobalDrawable(this);
            MobController.mobs.Remove(this);
        }

        public override void Update()
        {
            base.Update();

            HandleInput();

            if (!isResponding) return;

            mousePosition = Utils.GetMouseWorldPosition();
            aimDirection = (mousePosition - (Vector2)_shootingOffset.position).normalized;
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            _animator.SetFloat("aimDirX", aimDirection.x);
            _animator.SetFloat("aimDirY", aimDirection.y);

            HandleShooting();
        }
        public override void FixedUpdate()
        {
            rb.MovePosition(rb.position + config.speed * Time.deltaTime * inputDirection);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                foreach (var interaction in _interactables)
                    interaction.LostFocus();

                interactable.Focus();
                _interactables.Add(interactable);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                interactable.LostFocus();
                _interactables.Remove(interactable);
            }
        }

        public override void Draw()
        {
            if (transform == null) return;

            //kierunek patrzenia
            GLDraw.DrawRay(_shootingOffset.position, aimDirection * 1.5f, Color.blue);

            //Kolizja gracza
            GLDraw.DrawCircle((Vector2)_body.position + _circleCollider.offset, _circleCollider.radius, Color.green);
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

            if (Keyboard.current.iKey.wasPressedThisFrame) SwitchAutoFire();
            if (Keyboard.current.f3Key.wasPressedThisFrame) debugScreen.SwitchVisibility();

            if (Consts.IsFocusedOnMainMenu) return;
            //TU BINDY KTORE NIE MOGA DZIALAC NA MAIN MENU
        }
        private void HandleShooting()
        {
            if ((Mouse.current.leftButton.isPressed && !canvasHandle.isCanvasEnabled && !EventSystem.current.IsPointerOverGameObject()) || autoFire)
                shootingManager.Shoot(quickBar.weapon, aimAngle, "shoots");
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed || _interactables == null || _interactables.Count == 0) return;

            _interactables[^1].Interact(this);
        }

        public void HandleMovement(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && !(context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled) || !isResponding) return;
            if (Consts.IsFocusedOnUI || Consts.IsFocusedOnMainMenu) return;

            Vector2 _input = context.ReadValue<Vector2>();
            inputDirection.x = _input.x;
            inputDirection.y = _input.y;
            inputDirection = inputDirection.normalized;

            _animator.SetFloat("Speed", inputDirection.magnitude);

            if (context.phase == InputActionPhase.Performed)
            {

                if (context.phase == InputActionPhase.Canceled) return;

                if (canvasHandle.isCanvasEnabled) return;

                if (inputDirection != Vector2.zero)
                {
                    _animator.SetFloat("moveX", inputDirection.x);
                    _animator.SetFloat("moveY", inputDirection.y);
                }
            }
        }

        public override void OnDeath()
        {
            SetResponding(false);
            _circleCollider.enabled = false;
            canvasHandle.OpenWindow<DeathScreen>();
            _animator.SetTrigger("died");
            _animator.ResetTrigger("died");
            Utils.Log("DIED");
        }

        public void Restart()
        {
            transform.position = gameManager.worldManager.playerSpawnPosition.position;
            _circleCollider.enabled = true;
            _cameraController.Restart();
            shootingManager.Restart();
            healthManager.Restart();
            quickBar.ResetSlots();
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
            _animator.ResetTrigger("died");
            _animator.Rebind();
            canvasHandle.CloseWindow<DeathScreen>();
            isInLobby = true;
        }

        public void StopMovement()
        {
            inputDirection = Vector2.zero;
            _animator.SetFloat("Speed", 0);
        }

        public void SetWeapon(Weapon weapon)
        {
            quickBar.SetWeaponToSlot(weapon);
            shootingManager.Restart();
        }

        private void SwitchAutoFire()
        {
            autoFire = !autoFire;
        }
    }
}