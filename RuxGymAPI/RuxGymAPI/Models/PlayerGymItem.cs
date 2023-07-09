using System.Text.Json.Serialization;
namespace RuxGymAPI.Models
{
    public class PlayerGymItem
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int DumbbellPressItem { get; set; } = 0;
        public int AbsItem { get; set; } = 0;
        public int SquatItem { get; set; } = 0;
        public int DeadLiftItem { get; set; } = 0;
        public int BenchPressItem { get; set; } = 0;
        [JsonIgnore]
        public Guid PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }

    }
}
