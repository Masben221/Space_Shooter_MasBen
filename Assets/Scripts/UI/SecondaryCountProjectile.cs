using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class SecondaryCountProjectile : MonoBehaviour
    {
        [SerializeField] private Text m_Text;

        private int m_LastCount;

        void Update()
        {
            UpdateRocketcount();
        }

        private void UpdateRocketcount()
        {
            if (Player.Instance != null)
            {
                int currentCount = Player.Instance.ActiveShip.SecondaryAmmo;

                if (m_LastCount != currentCount)
                {
                    m_LastCount = currentCount;

                    m_Text.text = m_LastCount.ToString();
                }
            }
            else return;
        }
    }
}