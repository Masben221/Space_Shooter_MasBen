using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class TimeStats : MonoBehaviour
    {
        [SerializeField] private Text m_Text;
        private int m_LevelTime;

        void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            if (LevelController.Instance != null) 
            {
                m_LevelTime = (int)LevelController.Instance.LevelTime;
               m_Text.text = "Time: " + m_LevelTime.ToString();                
            }
        }
    }
}