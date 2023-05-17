using Newtonsoft.Json;

namespace RuxGymAPI.Models
{
    public class WorldTimeAPI
    {
        private readonly HttpClient _httpClient;

        public WorldTimeAPI()
        {
            _httpClient = new HttpClient();
        }

        public async Task<DateTime> GetCurrentDateTimeInTimeZone(string timeZone)
        {
            var url = $"https://timeapi.io/api/Time/current/zone?timeZone={timeZone}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var timeInfo = JsonConvert.DeserializeObject<TimeInfo>(json);
            var dateTime = timeInfo.DateTimeUtc;
            return dateTime;
        }

        private class TimeInfo
        {
            [JsonProperty("dateTime")]
            public DateTime DateTimeUtc { get; set; }
        }
    }
}
