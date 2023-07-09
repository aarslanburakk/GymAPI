using System.Text.Json.Serialization;

namespace RuxGymAPI.Models
{
    public class PlayerBoxing
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public ulong BoxPower { get; set; } = 0;
        public int BoxHighScore { get; set; } = 0;
        [JsonIgnore]
        public Guid PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }

    }
}
