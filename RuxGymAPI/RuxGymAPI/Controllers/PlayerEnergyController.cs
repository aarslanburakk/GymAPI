using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using RuxGymAPI.VMModels;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerEnergyController : ControllerBase
    {
        private readonly RuxGymDBcontext _context;

        public PlayerEnergyController(RuxGymDBcontext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerStat(string id)
        {
            var PlayerEnergy = await _context.PlayerEnergies.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(id));

            return Ok(PlayerEnergy);

        }

        [HttpPut("{userID}")]
        public async Task<IActionResult> UpdateEnergyDate(string userID, [FromBody] PlayerEnergyDataVM dataEnergy)
        {
            var existingData = await _context.PlayerEnergies.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(userID));
            if (existingData == null)
            {
                return Ok(false);
            }
            else
            {
                existingData.PlayerCurrentEnergy = dataEnergy.PlayerCurrentEnergy;
                existingData.StartEnergyTime = dataEnergy.StartEnergyTime;
                existingData.EndEnergyTime = dataEnergy.EndEnergyTime;
                await _context.SaveChangesAsync();
                return Ok(true);
            }

        }
        [HttpPut("updateEnergy/{updateEnergyID}")]
        public async Task<IActionResult> UpdateEnergy(string updateEnergyID, int currentEnergy)
        {
            var existingData = await _context.PlayerEnergies.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(updateEnergyID));
            if (existingData == null)
            {
                return Ok(false);
            }
            else
            {
                existingData.PlayerCurrentEnergy = currentEnergy;
                await _context.SaveChangesAsync();
                return Ok(true);
            }
        }



    }
}
