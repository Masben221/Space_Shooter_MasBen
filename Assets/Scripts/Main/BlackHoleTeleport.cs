using UnityEngine;
namespace SpaceShooter
{
    public class BlackHoleTeleport : MonoBehaviour
    {
        public static string IgnoreTag = "Asteroid";
        [SerializeField] private Transform m_TeleportPoint;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag(IgnoreTag)) return;
            collision.transform.position = m_TeleportPoint.position;
        }
    }
}
