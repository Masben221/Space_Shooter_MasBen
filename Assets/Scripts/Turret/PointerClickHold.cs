using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SpaceShooter
{
    public class PointerClickHold : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private bool m_Hold;
        public bool IsHold => m_Hold;

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_Hold = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_Hold = false;
        }
    }
}