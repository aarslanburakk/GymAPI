namespace RuxGymAPI.Repository.Tournament
{
    public interface ITournament
    {

        Task OlimpiaReward(string key);
        public void StartTime();
        public Task<int> GetOlympiaWeek();

        public TimeSpan TimeUntilNextMondayAtMidnight();
       
    }
}
