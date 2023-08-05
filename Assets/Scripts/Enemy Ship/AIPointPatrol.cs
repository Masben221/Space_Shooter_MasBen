using UnityEngine;

namespace SpaceShooter
{
    public class AIPointPatrol : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius { get  => m_Radius; set { m_Radius = value; } }

        private static readonly Color GizmoColor = new Color(1, 0, 0, 0.3f);

        /// <summary>
        /// ќтрисовывает границы зоны патрулировани€.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;

            Gizmos.DrawSphere(transform.position, m_Radius);
        }
    }
}
