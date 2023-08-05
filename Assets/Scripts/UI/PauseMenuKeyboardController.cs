using UnityEngine;

namespace SpaceShooter
{
    public class PauseMenuKeyboardController : MonoBehaviour
    {
        [SerializeField] private PauseMenuPanel m_PauseMenuPanel;

        [SerializeField] private GameObject m_ResultPanel;        
       
        void Update()
        {
            ControlKeyboard();
        }

        private void ControlKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_ResultPanel.activeSelf) return;

                if (!m_PauseMenuPanel.gameObject.activeSelf)
                {
                    m_PauseMenuPanel.OnButtonShowPause();
                }
                else
                    m_PauseMenuPanel.OnButtonContinue();
            }
        }
    }
}