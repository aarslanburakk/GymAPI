using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace RuxGymAPI.Models
{
    public class PlayerStat
    {
        public Guid ID { get; set; }
 
        public float ALlPower { get; set; }
        public float ArmPower   { get; set; }
        public float SixpackPower { get; set; }
        public float BackPower { get; set; }
        public float LegPower { get; set; }
        public float ChestPower { get; set; }   
        public int ProteinItem { get; set; }
        public int CreatinItem { get; set; }
        public int EnergyItem { get; set; }
        public int PlayerCash  { get; set; }

        public int PlayerDiamond { get; set; }
        public int PlayerSpinCount { get; set; }
        public int PlayerGoldTicket { get; set; }
        public int OlimpiaWin { get; set; }
        public bool IsOlimpia { get; set; }
        [DisplayName("Player")]
        public Guid UserId { get; set; }
        
    }
}
