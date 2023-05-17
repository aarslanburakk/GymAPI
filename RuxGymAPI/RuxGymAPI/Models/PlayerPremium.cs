namespace RuxGymAPI.Models
{
    public class PlayerPremium
    {

        public Guid Id { get; set; }
        public bool isPremium { get; set; }
        public string? EndPremiumDay { get; set; }

        public Guid UserId { get; set; }
    }
}
