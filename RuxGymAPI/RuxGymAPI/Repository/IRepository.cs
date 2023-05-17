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

        public Task<string> ChangePasswordAsync(ResetPasswordVM data);
        
    }
}
