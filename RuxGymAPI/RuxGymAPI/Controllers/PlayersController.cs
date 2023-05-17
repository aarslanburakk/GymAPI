using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using RuxGymAPI.Repository;
using RuxGymAPI.VMModels;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private static string key = "ruxgameearn1milliondolarin2024on";
        private readonly IRepository repository;
        private string code = null;
        
        public PlayersController(IRepository context)
        {
            repository = context;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerVM player)
        {

            var PlayerEmail = await repository.Players.FirstOrDefaultAsync(x => x.Email.Equals(player.Email));
            var PlayerUserName = await repository.Players.FirstOrDefaultAsync(x => x.UserName.Equals(player.UserName));
           
            if (PlayerEmail != null)
            {
                return Ok("Email Taken");
            }
            else if (PlayerUserName != null)
            {
                return Ok("Username Taken");
            }
            else
            {
                await repository.AddPlayerAsync(player);
                return Ok(true);
            }


        }



        [HttpGet]
        public async Task<IActionResult> LoginPlayer(string playerEmail, string playerPassword)
        {
            var currentUser = await repository.Players.FirstOrDefaultAsync(x => x.Email.Equals(playerEmail));

            if (currentUser != null)
            {
                string decryptedPassword = EfRepository.DecryptStringFromBytes_Aes(currentUser.Password, key);
                if (decryptedPassword == playerPassword)
                {
                    return Ok(currentUser);
                }
                else
                {
                    return Ok(false);
                }
            }
            else
            {
                return Ok("Email not found");
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> GetUser(string id, bool isOnline)
        {

            var resut = await repository.UpdatePlayerOnlineAsync(id, isOnline);
          return Ok(resut);
          
        }




        [HttpPost("email")]
        public async Task<IActionResult> ResetPasssword(string email)
        {
            return Ok(await repository.SendEmailCodeAsync(email));

        }
        [HttpPut("code")]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM data)
        {
            
            return Ok(await repository.ChangePasswordAsync(data));

        }



    }
}
