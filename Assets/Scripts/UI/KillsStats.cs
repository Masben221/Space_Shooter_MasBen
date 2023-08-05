using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class KillsStats : MonoBehaviour
    {
        [SerializeField] private Text m_Text;

        private int m_LastKills;

        void Update()
        {
            UpdateKills();
        }

        private void UpdateKills()
        {
            if (Player.Instance != null) 
            {
                int currentKills = Player.Instance.NumKills;

                if (m_LastKills != currentKills) 
                {
                    m_LastKills = currentKills;

                    m_Text.text = "Kills: " + m_LastKills.ToString();
                }
            }
        }
    }
}