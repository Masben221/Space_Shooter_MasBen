using Cinemachine;
using UnityEngine;

namespace SpaceShooter
{
    public class CameraShaker : MonoBehaviour
    {
        private CinemachineVirtualCamera m_Camera;
        [SerializeField] private float shakeAmount;
        private void Start()
        {
            m_Camera = Player.Instance.CameraController.Camera;
        }
        private void Update()
        {
            if (m_Camera)
            {
                m_Camera.transform.localPosition += Random.insideUnitSphere * shakeAmount * Time.deltaTime;
            }

        }
    }
}
