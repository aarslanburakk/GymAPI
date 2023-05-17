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
    public class StatsController : ControllerBase
    {

        private readonly RuxGymDBcontext _context;

        public StatsController(RuxGymDBcontext context)
        {
            _context = context;
        }

        // id and get player stats
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerStat(string id)
        {
            var playerstats = await _context.PlayerStats.FirstOrDefaultAsync(data => data.UserId == Guid.Parse(id));

            return Ok(playerstats);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutData(string id, [FromBody] UpdatePlayerStat data)
        {
            PlayerStat? existingData = await _context.PlayerStats.FirstOrDefaultAsync(data => data.UserId == Guid.Parse(id));
            if (existingData == null)
            {
                return NotFound();
            }
            else
            {
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
                existingData.PlayerDiamond = data.PlayerDiamond;
                existingData.PlayerSpinCount = data.PlayerSpinCount;
                existingData.PlayerGoldTicket = data.PlayerGoldTicket;
                await _context.SaveChangesAsync();
                return NoContent();
            }

        }
        [HttpPut]
        public async Task<IActionResult> OlimpiaUpdate(string MrOlimpiaPlayerID, bool isJoinedOlimpia)
        {
            var currentPlayer = await _context.PlayerStats.FirstOrDefaultAsync(data => data.UserId == Guid.Parse(MrOlimpiaPlayerID));
            if (currentPlayer == null)
            {
                return Ok(false);
            }
            else
            {

                currentPlayer.IsOlimpia = isJoinedOlimpia;
                await _context.SaveChangesAsync();
                return Ok(true);
            }

        }

        [HttpGet("UserAllPower")]
        public async Task<IActionResult> MrOlimpiaPlayer(string username)
        {
            IQueryable<PlayerStat> query = _context.PlayerStats;

            query = query.OrderByDescending(d => d.ALlPower);

            var result = await query
                .Select(x => new
                {
                    UserId = x.UserId,
                    UserName = _context.Players.Where(p => p.Id == x.UserId).Select(p => p.UserName).FirstOrDefault(),
                    ALlPower = x.ALlPower
                })
                .Take(100)
                .ToListAsync();

            // Add the rank property in-memory
            var newresult = result.Select((x, i) => new
            {
                Rank = i + 1,
                x.UserId,
                x.UserName,
                x.ALlPower
            })
               .ToList();
            var currentUser = newresult.Where(z => z.UserName == username);

            return Ok(new { GeneralRank = newresult, GeneralRankUser = currentUser });
        }


        [HttpGet("mrOlimpiaWinerList")]
        public async Task<IActionResult> MrOlimpiaWinners(string userName)
        {
            IQueryable<PlayerStat> query = _context.PlayerStats;

            query = query.OrderByDescending(d => d.OlimpiaWin);

            var result = await query
                .Select(x => new { UserName = _context.Players.Where(p => p.Id == x.UserId).Select(p => p.UserName).FirstOrDefault(), OlimpiaWin = x.OlimpiaWin })
                .ToListAsync();

            // Add the rank property in-memory
            var newResult = result.Select((x, i) => new
            {
                Rank = i + 1,
                x.UserName,
                x.OlimpiaWin
            })
             .ToList();
            var topTen = newResult.Take(10);
            var currentUser = newResult.Where(z => z.UserName == userName);

            return Ok(new { RankTen = topTen, RankUser = currentUser });

        }


        [HttpGet("Joiners")]
        public async Task<IActionResult> JoinersOlympia()
                {
            IQueryable<PlayerStat> query = _context.PlayerStats;
            query = query.OrderByDescending(c => c.ALlPower);
            var joiners = await query.Where(z => z.IsOlimpia.Equals(true)).ToListAsync();

            return Ok(joiners);

        }
        [HttpGet("count")]
        public async Task<IActionResult> JoinersOlimpiaCount()
        {
            IQueryable<PlayerStat> query = _context.PlayerStats;

            var joiners = await query.Where(z => z.IsOlimpia.Equals(true)).ToListAsync();

            return Ok(joiners.Count);

        }

    }
}
