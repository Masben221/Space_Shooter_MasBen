using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanelController : SingletonBase<ResultPanelController>
    {
        [SerializeField] private Text m_Kills;

        [SerializeField] private Text m_Score;

        [SerializeField] private Text m_Time;

        [SerializeField] private Text m_Result;

        [SerializeField] private Text m_ButtonNextText;

        //[SerializeField] private AnimationBase m_Target;

        private bool m_Success;

        private void Start()
        {
            gameObject.SetActive(false);

            //m_Target.OnEventEnd.AddListener(StopTime);
        }

        public void ShowResults(PlayerStatistics levelResults, bool success)
        {
            gameObject.SetActive(true);

            m_Success = success;

            m_Result.text = success ? "Level Completed" : "You Lose";

            m_Kills.text = "Kills : " + levelResults.NumKills.ToString();

            m_Score.text = "Score : " + levelResults.Score.ToString();

            m_Time.text = "Time : " + levelResults.Time.ToString();

            m_ButtonNextText.text = success ? "Next" : "Restart";

            Time.timeScale = 0;

            //m_Target.StartAnimation(true);
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;

            if (m_Success)
            {
                LevelSequenceController.Instance.AdvanceLevel();
            }
            else
            {
                LevelSequenceController.Instance.RestartLevel();
            }
        }

        private void StopTime()
        {
            Time.timeScale = 0;
        }
    }
}