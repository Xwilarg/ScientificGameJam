using ScientificGameJam.PowerUp;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScientificGameJam.UI
{
    public class PUDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("Prefab of the Text object to display on pointer hover with power-up description")]
        public GameObject messagePrefab;
        private GameObject messageInstance;

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
            if (messageInstance != null) Destroy(messageInstance);

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

            messageInstance = Instantiate(messagePrefab, _canvas);
            messageInstance.GetComponentInChildren<TMP_Text>().text = puManager.GetPowerupDescription(powerUpName);
            messageInstance.GetComponentInChildren<Image>().sprite = puManager.GetPowerupExpl(powerUpName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DestroyMessage();
        }

        public void DestroyMessage()
        {
            Destroy(messageInstance);
            messageInstance = null;
        }
    }
}