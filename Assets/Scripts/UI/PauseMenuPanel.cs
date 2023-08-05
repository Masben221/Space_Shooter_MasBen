using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class PauseMenuPanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_ResultPanel;

        //[SerializeField] private AnimationBase m_Target;
        private void Start()
        {
            gameObject.SetActive(false);

            //m_Target.OnEventEnd.AddListener(StopTime);
        }

        public void OnButtonShowPause()
        {
            if (m_ResultPanel.activeSelf) return;

            if (gameObject.activeSelf) return;
            

            gameObject.SetActive(true);

            Time.timeScale = 0;

            //m_Target.StartAnimation(true);
        }

        public void OnButtonContinue()
        {
            Time.timeScale = 1;

            gameObject.SetActive(false);
        }

        public void OnButtonMainMenu()
        {
            Time.timeScale = 1;

            gameObject.SetActive(false);

            LevelSequenceController.Instance.EpisodeStatistics = null;

            SceneManager.LoadScene(LevelSequenceController.MainMenuSceneNickName);
        }

        private void StopTime()
        {
            Time.timeScale = 0;
        }
    }
}