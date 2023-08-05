using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class UIHPProgress : MonoBehaviour
    {
        [SerializeField] private Image m_HPProgressBar;

        [SerializeField] private SpaceShip m_SpaceShip;

        private float m_FillAmountStep;
        public float FillAmountStep { get => m_FillAmountStep; set => m_FillAmountStep = value; }

        private void Start()
        {
            m_HPProgressBar.fillAmount = 1;

            FillAmountStep = 1f / (float)m_SpaceShip.HitPoints;

            m_SpaceShip.EventOnUpdateHP?.AddListener(UpdateHPProgress);
        }

        public void UpdateHPProgress(int HPCount)
        {
            m_HPProgressBar.fillAmount = HPCount * m_FillAmountStep;
        }

        public void UpdateShip(SpaceShip spaceShip)
        {
            m_SpaceShip = spaceShip;

            m_SpaceShip.EventOnUpdateHP?.AddListener(UpdateHPProgress);
        }
    }
}