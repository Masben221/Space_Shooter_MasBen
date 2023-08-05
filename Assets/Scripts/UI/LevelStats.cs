using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LevelStats : MonoBehaviour
    {
        [SerializeField] private Text m_LevelName;
        public Text LevelName { get => m_LevelName; set => m_LevelName = value; }

        [SerializeField] private Text m_Kills_Level;
        public Text Kills_Level { get => m_Kills_Level; set => m_Kills_Level = value; }

        [SerializeField] private Text m_Score_Level;
        public Text Score_Level { get => m_Score_Level; set => m_Score_Level = value; }

        [SerializeField] private Text m_Time_Level;
        public Text Time_Level { get => m_Time_Level; set => m_Time_Level = value; }
    }
}