using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickName = "Main_Menu";
        public Episode CurrentEpisode { get; private set; } //Текущий эпизод.

        public int CurrentLevel { get; private set; } //Текущий уровень.

        public bool LastLevelResult { get; private set; } // 

        public PlayerStatistics LevelStatistics { get; private set; } //Статистика пройденного уровня.
        public EpisodeStatistics EpisodeStatistics { get; set; } //Статистика эпизода.

        public static SpaceShip PlayerShip { get; set; }

        private int m_BonusIndex = 2;

        public int EpisodeCount { get; private set; } = 1; //Колличество пройденных эпизодов.

        //Старт эпизода.
        public void StartEpisode(Episode episode)
        {
            CurrentEpisode = episode;

            CurrentLevel = 0;

            //сбрасываем статы перед началом эпизода.
            LevelStatistics = new PlayerStatistics();

            LevelStatistics.Reset();

            EpisodeStatistics = new EpisodeStatistics(CurrentEpisode.Levels.Length, CurrentEpisode.EpisodeName);

            EpisodeStatistics.Reset();

            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }
        
        //Рестарт уровня.
        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        //Завершение прохождения уровня.
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

        //Переход на другой  уровень если он есть, а иначе завершение эпизода и выход в главное меню.
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
        //Подсчет статистики уровня.
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