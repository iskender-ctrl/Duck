using System;
using UnityEngine.EventSystems;

namespace Player.CannonControl
{
    public class LimitationsClickDetector : UIBehaviour, IPointerClickHandler
    {
        public event Action onClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClicked?.Invoke();
        }
    }
}