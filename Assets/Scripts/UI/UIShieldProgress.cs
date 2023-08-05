using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class UIShieldProgress : MonoBehaviour
    {
        [SerializeField] private Image m_ShildProgressBar;

        [SerializeField] private SpaceShip m_SpaceShip;

        private float m_FillAmountStep;
        public float FillAmountStep { get => m_FillAmountStep; set => m_FillAmountStep = value; }

        private void Start()
        {
            m_ShildProgressBar.fillAmount = 1;

            FillAmountStep = 1f / (float)m_SpaceShip.MaxShield;

            m_SpaceShip.EventOnUpdateShield?.AddListener(UpdateShieldProgress);
        }

        public void UpdateShieldProgress(float count)
        {
            m_ShildProgressBar.fillAmount = count * m_FillAmountStep;
        }

        public void UpdateShip(SpaceShip spaceShip)
        {
            m_SpaceShip = spaceShip;

            m_SpaceShip.EventOnUpdateShield?.AddListener(UpdateShieldProgress);
        }
    }
}