namespace RuxGymAPI.VMModels
{
    public class CreatePlayerVM
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsGuest { get; set; }

        public bool IsFacebookUser { get; set; }
        public string? FacebookId { get; set; }

    }
}
