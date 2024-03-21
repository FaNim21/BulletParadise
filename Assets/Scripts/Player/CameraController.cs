using UnityEngine;

namespace BulletParadise.Player
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        public Transform target;
        public bool isTargeting;

        [Header("Debug")]
        [Range(0f, 1f)] public float smoothSpeed = 0.125f;
        public Vector2 offset;

        private Vector3 _velocity = Vector3.zero;


        public void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            target = PlayerController.Instance.transform;
            transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10);
        }

        private void LateUpdate()
        {
            if (!isTargeting || target == null) return;

            Vector3 desiredPosition = new(target.position.x + offset.x, target.position.y + offset.y, -10);
            Vector3 positionSmoothed = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);

            transform.position = positionSmoothed;
        }

        public void ChangeTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}