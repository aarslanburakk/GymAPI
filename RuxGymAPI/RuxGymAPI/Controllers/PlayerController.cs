using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using System.Net;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private readonly RuxGymDBcontext _context;

        public PlayerController(RuxGymDBcontext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Player player)
        {   

            var PlayerEmail = _context.Players.FirstOrDefault(x => x.Email.Equals(player.Email));
            var PlayerUserName = _context.Players.FirstOrDefault(x => x.UserName.Equals(player.UserName));
            
            if (PlayerEmail != null || PlayerUserName != null)
            {
                return Ok(null);
            }
            else
            {
                await _context.Players.AddAsync(new()
                {
                    Id = player.Id,
                    Email = player.Email,
                    UserName = player.UserName,
                    Password = player.Password,                   
                    CreatedDate = player.CreatedDate

                }) ;
                await _context.PlayerStats.AddAsync(new()
                {
                    ID = Guid.NewGuid(),
                    ALlPower = 50,
                    ArmPower = 10,
                    SixpackPower = 10,
                    BackPower = 10,
                    LegPower = 10,
                    ChestPower = 10,
                    ProteinItem = 0,
                    CreatinItem = 0,
                    EnergyItem = 0,
                    PlayerCash = 0,
                    UserId = player.Id

                }); 
                await _context.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.Created);
            }


        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allUsers = _context.Players.ToList();
            return Ok(allUsers);
        }

        // id and get player stats
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var playerstats = await _context.PlayerStats.FirstOrDefaultAsync(data => data.UserId == Guid.Parse(id));

            return Ok(playerstats);

        }
        //d

        [HttpPut("{id}")]
        public async Task<IActionResult> PutData(string id, [FromBody] PlayerStat data)
        {
            PlayerStat existingData = await _context.PlayerStats.FirstOrDefaultAsync(data => data.UserId == Guid.Parse(id));
            if (existingData == null)
            {
                return NotFound();
            }
            existingData.ALlPower = data.ALlPower;
            existingData.ArmPower = data.ArmPower;
            existingData.SixpackPower = data.SixpackPower;
            existingData.BackPower = data.BackPower;
            existingData.LegPower = data.LegPower;
            existingData.ChestPower = data.ChestPower;
            existingData.ProteinItem = data.ProteinItem;
            existingData.CreatinItem = data.CreatinItem;
            existingData.EnergyItem = data.EnergyItem;
            existingData.PlayerCash = data.PlayerCash;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
