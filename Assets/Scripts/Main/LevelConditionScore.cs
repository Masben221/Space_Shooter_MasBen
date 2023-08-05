using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionScore : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private int m_Score;

        private bool m_Reached; //Есть ли завершение.

        public bool IsCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if (Player.Instance.Score >= m_Score) 
                    {
                        m_Reached = true;
                    }
                }

                return m_Reached;
            }
        }
    }
}