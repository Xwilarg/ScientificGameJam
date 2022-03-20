using ScientificGameJam.PowerUp;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScientificGameJam.UI
{
    public class PUSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
    {
        public PowerUpManager puManager;
        public int slotNumber;

        private GameObject powerUp;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                PUDragHandler pu = eventData.pointerDrag.GetComponent<PUDragHandler>();

                if (!puManager.ContainsPowerup(pu.powerUpName))
                {
                    if (powerUp != null) Destroy(powerUp);
                    
                    puManager.AddPowerup(slotNumber, pu.powerUpName);

                    powerUp = Instantiate(eventData.pointerDrag, transform);
                    RectTransform rect = powerUp.GetComponent<RectTransform>();
                    rect.anchorMin = new Vector2(0.5f, 0.5f);
                    rect.anchorMax = new Vector2(0.5f, 0.5f);
                    rect.pivot = new Vector2(0.5f, 0.5f);
                    rect.localPosition = Vector3.zero;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (powerUp != null)
            {
                powerUp.GetComponent<PUDragHandler>().DestroyMessage();
                Destroy(powerUp);
            }
            puManager.RemovePowerup(slotNumber);
        }
    }
}
