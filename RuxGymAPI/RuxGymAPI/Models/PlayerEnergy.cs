using System.Text.Json.Serialization;

namespace RuxGymAPI.Models
{
    public class PlayerEnergy
    {
        [JsonIgnore]
        public Guid Id { get; set; }  = Guid.NewGuid(); 
        public int PlayerCurrentEnergy { get; set; } = 100;
        public string StartEnergyTime { get; set; }
        public string EndEnergyTime { get; set; }
        [JsonIgnore]
        public Guid PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }


    }
}
