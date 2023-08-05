using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class StatisticsPanelController : SingletonBase<StatisticsPanelController>
    {
        [SerializeField] private Text m_EpisodeName;

        [SerializeField] private Text m_Attempts;

        [SerializeField] private Text m_Deaths;

        [SerializeField] private Text m_LevelCompleted;

        [SerializeField] private Text m_TotalTime;

        [SerializeField] private Transform m_LevelStatsPoint;

        [SerializeField] private GameObject m_LevelPrefabe;

        //[SerializeField] private AnimationBase m_Target;

        private List<GameObject> m_levelStats = new List<GameObject>();

        public void RecordResults()
        {
            if (LevelSequenceController.Instance.EpisodeStatistics != null)
            {
                var EpisodeResults = LevelSequenceController.Instance.EpisodeStatistics;

                m_EpisodeName.text = EpisodeResults.EpisodeName;

                m_Attempts.text = "Attempts : " + EpisodeResults.NumberOfAttempts.ToString();

                m_Deaths.text = "Deaths : " + EpisodeResults.NumberOfDeaths.ToString();

                m_LevelCompleted.text = "Level Completed : " + EpisodeResults.CompletedLevels.ToString();

                var totalTime = 0;

                for (int i = 0; i < EpisodeResults.LevelStatistics.Length; i++)
                {
                    var newLevel = Instantiate(m_LevelPrefabe, m_LevelStatsPoint.position, Quaternion.identity, m_LevelStatsPoint);

                    m_levelStats.Add(newLevel);

                    totalTime += EpisodeResults.LevelStatistics[i].Time;

                    newLevel.GetComponent<LevelStats>().LevelName.text = "Level : " + (i + 1);

                    newLevel.GetComponent<LevelStats>().Kills_Level.text = "Kills/Destroy : " + EpisodeResults.LevelStatistics[i].NumKills.ToString();

                    newLevel.GetComponent<LevelStats>().Score_Level.text = "Score : " + EpisodeResults.LevelStatistics[i].Score.ToString();

                    newLevel.GetComponent<LevelStats>().Time_Level.text = "Time : " + EpisodeResults.LevelStatistics[i].Time.ToString();

                }

                m_TotalTime.text = "Total Time : " + totalTime.ToString();

                //m_Target.StartAnimation(true);
            }
        }

        public void OnButtonBackToMenu()
        {
            foreach (var level in m_levelStats)
            {
                Destroy(level.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}