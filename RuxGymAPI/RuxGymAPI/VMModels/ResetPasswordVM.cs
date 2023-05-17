namespace RuxGymAPI.VMModels
{
    public class ResetPasswordVM
    {
        public string UserEmail { get; set; }

        public string CodeKey { get; set; }
        public string NewPassword { get; set; }


    }
}
