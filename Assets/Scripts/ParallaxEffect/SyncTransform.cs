using UnityEngine;
namespace SpaceShooter
{
    /// <summary>
    /// Закрепление перемещения текущего объекта за target
    /// </summary>
    public class SyncTransform : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private void FixedUpdate()
        {
            transform.position = new Vector3(m_Target.position.x, m_Target.position.y, transform.position.z);
        }
    }
}
