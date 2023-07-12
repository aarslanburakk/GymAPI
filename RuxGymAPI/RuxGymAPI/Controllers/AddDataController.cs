using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuxGymAPI.Context;
using RuxGymAPI.Models;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDataController : ControllerBase
    {
        private readonly RuxGymDBcontext _context;

        public AddDataController(RuxGymDBcontext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> data()
        {
            List<Guid> list = new List<Guid>();

            list = _context.Players.Select(x => x.Id).ToList();
            foreach (var item in list)
            {
                var result = _context.PlayerBoxings.FirstOrDefault(c => c.PlayerId.Equals(item));
                if (result == null)
                {
                    await _context.PlayerBoxings.AddAsync(new PlayerBoxing
                    {
                        PlayerId = item

                    });
                }
            }
            await   _context.SaveChangesAsync();
            return Ok();

        }

    }
}
