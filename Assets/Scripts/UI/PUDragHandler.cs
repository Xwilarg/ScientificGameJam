using ScientificGameJam.PowerUp;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScientificGameJam.UI
{
    public class PUDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        private RectTransform _canvas; //UI Canvas
        private Vector2 _offset;

        private GameObject draggedObject; //The object being dragged
        private CanvasGroup canvasGroup;
        public string powerUpName;

        public void Awake()
        {
            _canvas = (RectTransform) GetComponentInParent<Canvas>().transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            _offset = (Vector2)transform.position - mousePosition;

            draggedObject = Instantiate(this.gameObject, _offset, Quaternion.identity, _canvas);
            canvasGroup = draggedObject.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            draggedObject.transform.position = mousePosition + _offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            Destroy(draggedObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PowerUpManager puManager = FindObjectOfType<PowerUpManager>();

            UnityEngine.Debug.Assert(puManager != null);
            UnityEngine.Debug.Log(puManager.GetPowerupDescription(powerUpName));
        }
    }
}