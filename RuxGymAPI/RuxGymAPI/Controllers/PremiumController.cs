using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.VMModels;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumController : ControllerBase
    {

        private readonly RuxGymDBcontext _context;

        public PremiumController(RuxGymDBcontext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPremiumStats(string id)
        {
            var playerPremium = await _context.PlayerPremia.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(id));

            return Ok(playerPremium);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePremiumDate(string id, [FromBody] PlayerPremiumVM data)
        {
            var playerPremium = await _context.PlayerPremia.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(id));
            if (playerPremium == null)
            {
                return NotFound();
            }
            playerPremium.IsPremium = data.IsPremium;
            playerPremium.EndPremiumDay = data.EndPremiumDay;
            await _context.SaveChangesAsync();
            return Ok(playerPremium);

        }


    }
}
