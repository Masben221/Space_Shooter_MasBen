using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionsPosition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private ScopeOfAction m_ScopeAction;

        private bool m_Reached; //���� �� ����������.

        public bool IsCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    //���� ���������� ���� ��������.
                    if (m_ScopeAction.IsAction)
                    {
                        m_Reached = true;
                    } 
                    else
                        m_Reached = false;
                }

                return m_Reached;
            }
        }
    }
}