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
        public QuickBar quickBar;
        public CameraController cameraController;
        public CanvasHandle canvasHandle;
        public LevelLoader levelLoader;
        public ShootingManager shootingManager;

        [SerializeField] private Animator animator;
        [SerializeField] private CircleCollider2D circleCollider;

        [Header("Obiekty")]
        public Transform body;
        public Transform shootingOffset;

        [Header("G³ówne wartoœci")]
        public float speed;

        [Header("Debug")]
        public bool autoFire;
        [ReadOnly] public bool isInLobby;
        [ReadOnly] public bool isResponding;
        [ReadOnly] public float aimAngle;
        [ReadOnly] public Vector2 inputDirection;
        [ReadOnly] public Vector2 mousePosition;
        [ReadOnly] public Vector2 aimDirection;

        private readonly List<IInteractable> interactables = new();


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

            //kierunek patrzenia
            GLDraw.DrawRay(shootingOffset.position, aimDirection * 1.5f, Color.blue);

            //Kolizja gracza
            GLDraw.DrawCircle((Vector2)body.position + circleCollider.offset, circleCollider.radius, Color.green);
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
            if ((Mouse.current.leftButton.isPressed && !canvasHandle.isCanvasEnabled && !EventSystem.current.IsPointerOverGameObject()) || autoFire)
                shootingManager.Shoot(quickBar.weapon, aimAngle);
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed || interactables == null || interactables.Count == 0) return;

            interactables[^1].Interact(this);
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

        public override void OnDeath()
        {
            SetResponding(false);
            circleCollider.enabled = false;
            canvasHandle.OpenWindow<DeathScreen>();
            animator.SetTrigger("died");
            Utils.Log("DIED");
        }

        public void Restart()
        {
            transform.position = Vector2.zero;
            circleCollider.enabled = true;
            cameraController.Restart();
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
            shootingManager.Restart();
        }
    }
}