using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceShooter
{
    /// <summary>
    /// Задаем область спавна
    /// </summary>
    public class CircleArea : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        [SerializeField] private Color m_Color;

        public Vector2 GetRandomInsideZone()// метод получения координат рандомной точки в окружности радиуса m_Radius
        {
            return (Vector2)transform.position + (Vector2)UnityEngine.Random.insideUnitSphere * m_Radius;
        }

        #if UNITY_EDITOR
        //private static Color m_GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected() // отрисовка закрашенной окружности в текущих координатах радиуса m_Radius
        {
            Handles.color = m_Color;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
        #endif
    }
}