using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Притяжение объекта к текущему объекту
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class GravityWell : MonoBehaviour
    {
        [SerializeField] private float m_Force;
        [SerializeField] private float m_Radius;

        private void OnTriggerStay2D(Collider2D collision)
        {
            //если у колизии нет Rigidbody 
            if (collision.attachedRigidbody == null) return;

            //получаем направление притяжения
            Vector2 dir = transform.position - collision.transform.position;
            float dist = dir.magnitude;
            //if(dist > m_Radius)
            //{
            Vector2 force = m_Force * dir.normalized * (m_Radius / dist);
                collision.attachedRigidbody.AddForce(force, ForceMode2D.Force);
            //}
        }

        //срабатывает при изменении значений
        #if UNITY_EDITOR
        private void OnValidate()
        {
            GetComponent<CircleCollider2D>().radius = m_Radius;
        }
        #endif
    }
}