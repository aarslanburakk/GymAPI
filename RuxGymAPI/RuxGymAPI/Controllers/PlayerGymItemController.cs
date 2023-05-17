using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.VMModels;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerGymItemController : ControllerBase
    {

        private readonly RuxGymDBcontext _context;

        public PlayerGymItemController(RuxGymDBcontext context)
        {
            _context = context;
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerItem(string id)
        {
            var gymItem = await _context.PlayerGymItems.FirstOrDefaultAsync(data => data.UserID == Guid.Parse(id));

            return Ok(gymItem);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerItem(string id, PlayerGymItemVM gymItemData)
        {
            var existingData = await _context.PlayerGymItems.FirstOrDefaultAsync(data => data.UserID == Guid.Parse(id));
            if (existingData == null)
            {
                return Ok(false);
            }
            else
            {

                existingData.DumbbellPressItem = gymItemData.DumbbellPressItem;
                existingData.AbsItem = gymItemData.AbsItem;
                existingData.SquatItem = gymItemData.SquatItem;
                existingData.DeadLiftItem = gymItemData.DeadLiftItem;
                existingData.BenchPressItem = gymItemData.BenchPressItem;
                await _context.SaveChangesAsync();
                return Ok(true);
            }

        }

    }
}
