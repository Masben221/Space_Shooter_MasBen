using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickName = "Main_Menu";
        public Episode CurrentEpisode { get; private set; } //������� ������.

        public int CurrentLevel { get; private set; } //������� �������.

        public bool LastLevelResult { get; private set; } // 

        public PlayerStatistics LevelStatistics { get; private set; } //���������� ����������� ������.
        public EpisodeStatistics EpisodeStatistics { get; set; } //���������� �������.

        public static SpaceShip PlayerShip { get; set; }

        private int m_BonusIndex = 2;

        public int EpisodeCount { get; private set; } = 1; //����������� ���������� ��������.

        //����� �������.
        public void StartEpisode(Episode episode)
        {
            CurrentEpisode = episode;

            CurrentLevel = 0;

            //���������� ����� ����� ������� �������.
            LevelStatistics = new PlayerStatistics();

            LevelStatistics.Reset();

            EpisodeStatistics = new EpisodeStatistics(CurrentEpisode.Levels.Length, CurrentEpisode.EpisodeName);

            EpisodeStatistics.Reset();

            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }
        
        //������� ������.
        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        //���������� ����������� ������.
        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;

            if (!success)
            {
                EpisodeStatistics.NumberOfAttempts++;
            }

            CalculateLevelStatistics();

            ResultPanelController.Instance.gameObject.SetActive(true);
            ResultPanelController.Instance.ShowResults(LevelStatistics, success);

        }

        //������� �� ������  ������� ���� �� ����, � ����� ���������� ������� � ����� � ������� ����.
        public void AdvanceLevel()
        {
            CurrentLevel++;

            EpisodeStatistics.CompletedLevels++;

            EpisodeStatistics.LevelStatistics[CurrentLevel - 1].NumKills = LevelStatistics.NumKills;

            EpisodeStatistics.LevelStatistics[CurrentLevel - 1].Score = LevelStatistics.Score;

            EpisodeStatistics.LevelStatistics[CurrentLevel - 1].Time = LevelStatistics.Time;

            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                EpisodeCount++;

                SceneManager.LoadScene(MainMenuSceneNickName);
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);

                LevelStatistics.Reset();
            }
        }
        //������� ���������� ������.
        public void CalculateLevelStatistics()
        {
            LevelStatistics.NumKills = Player.Instance.NumKills;

            LevelStatistics.Time = (int)LevelController.Instance.LevelTime;

            if (LevelStatistics.Time < LevelController.Instance.ReferenceTime)
            {
                LevelStatistics.Score = Player.Instance.Score * m_BonusIndex;
            }
            else
                LevelStatistics.Score = Player.Instance.Score;
        }
    }
}