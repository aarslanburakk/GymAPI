namespace RuxGymAPI.Models
{
    public class PasswordCode
    {
        public Guid Id { get; set; }
        public string PlayerId { get; set; }

        public string CodeKey { get; set; }
        public DateTime CreatedTime { get; set; }


    }
}
