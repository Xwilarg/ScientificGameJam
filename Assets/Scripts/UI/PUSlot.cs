using UnityEngine;
using UnityEngine.EventSystems;

namespace ScientificGameJam.UI
{
    public class PUSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
    {
        private GameObject powerUp;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if (powerUp != null) Destroy(powerUp);

                powerUp = Instantiate(eventData.pointerDrag, transform);            
                RectTransform rect = powerUp.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.localPosition = Vector3.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (powerUp != null) Destroy(powerUp);
        }
    }
}
