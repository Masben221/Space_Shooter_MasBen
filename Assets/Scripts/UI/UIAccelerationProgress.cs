using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class UIAccelerationProgress : MonoBehaviour
    {
        [SerializeField] private Image m_AccelerationProgressBar;

        [SerializeField] private SpaceShip m_SpaceShip;

        private float m_FillAmountStep;
        public float FillAmountStep { get => m_FillAmountStep; set => m_FillAmountStep = value; }

        private void Start()
        {
            m_AccelerationProgressBar.fillAmount = 1;

            FillAmountStep = 1f / (float)m_SpaceShip.MaxAcceleration;

            m_SpaceShip.EventOnUpdateAcceleration?.AddListener(UpdateAccelerationProgress);
        }

        public void UpdateAccelerationProgress(float count)
        {
            m_AccelerationProgressBar.fillAmount = count * m_FillAmountStep;
        }

        public void UpdateShip(SpaceShip spaceShip)
        {
            m_SpaceShip = spaceShip;

            m_SpaceShip.EventOnUpdateAcceleration?.AddListener(UpdateAccelerationProgress);
        }
    }
}