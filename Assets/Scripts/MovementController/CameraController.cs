using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// ������� �������� � ������� ������ �� ��������
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera; //�������� ������
        [SerializeField] private Camera m_MiniMapCamera; //������ ���������.
        [SerializeField] private Transform m_Target; //���� - ������� ������
        /// <summary>
        /// �������� �������� ������ �� target
        /// </summary>
        [SerializeField] private float m_InterpolationLinear;
        /// <summary>
        /// �������� �������� ������ �� target
        /// </summary>
        [SerializeField] private float m_InterpolationAngular;
        /// <summary>
        /// �������� ������ �� ��� UP
        /// </summary>
        [SerializeField] private float m_ForwardOffset;
        /// <summary>
        /// ���������� ������ �� ������� �� Z
        /// </summary>
        [SerializeField] private float m_CameraZOffset;

        [SerializeField] private float m_ZoomSpeed; //�������� ������������.

        [SerializeField] private float m_minZoomSize; //����������� ������ ����.

        [SerializeField] private float m_maxZoomSize; //������������ ������ ����.

        private void FixedUpdate()
        {
            if (m_Target == null || m_Camera == null) return;

            Vector2 camPos = m_Camera.transform.position;
            Vector2 targetPos = m_Target.position + m_Target.transform.up * m_ForwardOffset;
            Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);
            m_Camera.transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

            m_MiniMapCamera.transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

            if (m_InterpolationAngular > 0)
            {
                m_Camera.transform.rotation = Quaternion.Slerp(m_Camera.transform.rotation, 
                                                               m_Target.rotation, m_InterpolationAngular * Time.deltaTime);

                m_MiniMapCamera.transform.rotation = Quaternion.Slerp(m_MiniMapCamera.transform.rotation,
                                                               m_Target.rotation, m_InterpolationAngular * Time.deltaTime);

            }
            //��� ������ � ����������� �� ��������.
            if (m_Target.gameObject.transform.root.GetComponent<SpaceShip>().ThrustControl == 0)
            {
                m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, m_minZoomSize, Time.deltaTime * m_ZoomSpeed);
            }
            else
            {
                m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, m_maxZoomSize, Time.deltaTime * m_ZoomSpeed);
            }

        }
        public void SetTarget(Transform newTarget)
        {
            m_Target = newTarget;
        }
    }
}

