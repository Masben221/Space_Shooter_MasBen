namespace SpaceShooter
{
    public class EpisodeStatistics
    {
        public string EpisodeName;

        public PlayerStatistics[] LevelStatistics;

        public int NumberOfAttempts;

        public int NumberOfDeaths;

        public int CompletedLevels;

        public EpisodeStatistics(int levelStatisticsCount, string episodeName)
        {
            LevelStatistics = new PlayerStatistics[levelStatisticsCount];

            EpisodeName = episodeName;

            for (int i = 0; i < LevelStatistics.Length; i++)
            {
                LevelStatistics[i] = new PlayerStatistics();
            }
        }

        public void Reset()
        {
            foreach (var level in LevelStatistics)
            {
                level.Reset();
            }

            NumberOfAttempts = 1;

            NumberOfDeaths = 0;

            CompletedLevels = 0;
        }
    }
}