using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Models;
using RuxGymAPI.VMModels;

namespace RuxGymAPI.Repository
{
    public interface IRepository
    {
        IQueryable<Player> Players { get; }
      

        public  Task AddPlayerAsync(CreatePlayerVM data);

        public Task<bool> UpdatePlayerOnlineAsync(string id, bool isOnline);
        public Task<bool> SendEmailCodeAsync(string email);

        public Task<bool> MergeFacebookUser(string id, string facebookId);
        public Task<bool> ChangeUserName(string id, string userName);
        public Task<Player> GetFacebookUser(string facebookId);
        public Task MergeGuestUser(string id, CreatePlayerVM data);

        public Task<string> ChangePasswordAsync(ResetPasswordVM data);
        
    }
}
