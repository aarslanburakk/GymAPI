using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.VMModels;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpinTimeController : ControllerBase
    {
        private readonly RuxGymDBcontext _context;

        public SpinTimeController(RuxGymDBcontext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerStat(string id)
        {
            var PlayerEnergy = await _context.SpinDateTimes.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(id));

            return Ok(PlayerEnergy);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutData(string id,PlayerSpinDataVM spinData)
        {
            var existingData = await _context.SpinDateTimes.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(id));
            if (existingData == null)
            {
                return Ok(false);
            }
            else
            {
                
                existingData.CreatedSpinTime = spinData.EndSpinTime;
                await _context.SaveChangesAsync();
                return Ok(true);
            }

        }
    }
}
