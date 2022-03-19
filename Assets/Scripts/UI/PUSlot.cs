using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScientificGameJam.UI
{
    public class PUSlot : MonoBehaviour, IDropHandler
    {
        GameObject powerUp;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if (powerUp != null) Destroy(powerUp);
                powerUp = Instantiate(eventData.pointerDrag, transform);
                powerUp.transform.localPosition = Vector3.zero;
            }
        }
    }
}
