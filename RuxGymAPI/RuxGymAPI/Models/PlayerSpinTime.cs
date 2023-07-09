using System.Text.Json.Serialization;
namespace RuxGymAPI.Models
{
    public class PlayerSpinTime
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ? CreatedSpinTime { get; set; }
        [JsonIgnore]
        public Guid PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }
    }
}
