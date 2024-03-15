using UnityEngine;
using UnityEngine.EventSystems;

namespace BulletParadise.UI.Interactable
{
    public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public Canvas canvas;
        private RectTransform _dragRectTransform;

        [Header("Debug")]
        public bool isDraggable = true;


        public void Awake()
        {
            _dragRectTransform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDraggable)
                _dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isDraggable)
                _dragRectTransform.SetAsLastSibling();
        }

        public void CloseWindow()
        {
            _dragRectTransform.gameObject.SetActive(false);
        }
    }
}