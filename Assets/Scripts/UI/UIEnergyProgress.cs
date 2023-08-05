using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class UIEnergyProgress : MonoBehaviour
    {
        [SerializeField] private Image m_EnergyProgressBar;

        [SerializeField] private SpaceShip m_SpaceShip;

        private float m_FillAmountStep;
        public float FillAmountStep { get => m_FillAmountStep; set => m_FillAmountStep = value; }

        private void Start()
        {
            m_EnergyProgressBar.fillAmount = 1;

            FillAmountStep = 1f / (float)m_SpaceShip.MaxEnergy;

            m_SpaceShip.EventOnUpdateEnergy?.AddListener(UpdateEnergyProgress);
        }

        public void UpdateEnergyProgress(float energyCount)
        {
            m_EnergyProgressBar.fillAmount = energyCount * m_FillAmountStep;
        }

        public void UpdateShip(SpaceShip spaceShip)
        {
            m_SpaceShip = spaceShip;

            m_SpaceShip.EventOnUpdateEnergy?.AddListener(UpdateEnergyProgress);
        }
    }
}