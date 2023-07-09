using System.Text.Json.Serialization;

namespace RuxGymAPI.Models
{
    public class PlayerStat
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        public float ALlPower { get; set; } = 50;
        public float ArmPower { get; set; } = 10;
        public float SixpackPower { get; set; } = 10;
        public float BackPower { get; set; } = 10;
        public float LegPower { get; set; } = 10;
        public float ChestPower { get; set; } = 10;
        public int ProteinItem { get; set; } = 0;
        public int CreatinItem { get; set; } = 0;
        public int EnergyItem { get; set; } = 0;
        public int PlayerCash { get; set; } = 35000;
        public int PlayerDiamond { get; set; } = 10;
        public int PlayerSpinCount { get; set; } = 5;
        public int PlayerGoldTicket { get; set; } = 0;
        public int OlimpiaWin { get; set; } = 0;
        public bool IsOlimpia { get; set; } = false;
        [JsonIgnore]
        public Guid PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }

    }
}
