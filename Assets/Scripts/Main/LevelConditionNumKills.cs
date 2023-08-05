using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionNumKills : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private int m_NumKills;

        private bool m_Reached; //Есть ли завершение.

        public bool IsCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if (Player.Instance.NumKills >= m_NumKills) 
                    {
                        m_Reached = true;
                    }
                }

                return m_Reached;
            }
        }
    }
}