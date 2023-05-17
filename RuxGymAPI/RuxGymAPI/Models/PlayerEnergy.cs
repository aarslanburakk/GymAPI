namespace RuxGymAPI.Models
{
    public class PlayerEnergy
    {
        public Guid Id { get; set; }
        public int PlayerCurrentEnergy { get; set; }
        public string StartEnergyTime { get; set; }
        public string EndEnergyTime { get; set; }

        public Guid PlayerId { get; set; }
        

    }
}
