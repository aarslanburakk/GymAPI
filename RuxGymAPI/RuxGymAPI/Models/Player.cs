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
        
       
    }
}
