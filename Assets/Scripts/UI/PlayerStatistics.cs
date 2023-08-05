namespace SpaceShooter
{
    public class PlayerStatistics
    {
        public int NumKills;

        public int Score;

        public int Time;

        public void Reset()
        {
            NumKills = 0;

            Score = 0;

            Time = 0;
        }
    }
}