namespace RuxGymAPI.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public string CreatedDate { get; set; }
        public string? LastConnectionDate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGuest { get; set; }
        public bool IsFacebookUser { get; set; }
        public string? FacebookId { get; set; }
        
        public PlayerStat PlayerStat { get; set; }
        public PlayerEnergy PlayerEnergy { get; set; }
        public PlayerSpinTime PlayerSpinTime { get; set; }
        public PlayerGymItem PlayerGymItem { get; set; }
        public PlayerPremium PlayerPremium { get; set; }
        public PlayerBoxing PlayerBoxing { get; set; }
       
       
    }
}
