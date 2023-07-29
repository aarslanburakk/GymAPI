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

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerStat(string id, UpdatePlayerStat data)
        {
            PlayerStat? existingData = await _context.PlayerStats.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(id));
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
            var currentPlayer = await _context.PlayerStats.FirstOrDefaultAsync(data => data.PlayerId == Guid.Parse(MrOlimpiaPlayerID));
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


        [HttpGet("BoxingData")]
        public async Task<IActionResult> UpdatePlayerStat(string userId)
        {
            var data = await _context.PlayerBoxings.FirstOrDefaultAsync(c => c.PlayerId == Guid.Parse(userId));

            return Ok(data);
        }

        [HttpPut("UpdateBoxData")]
        public async Task<IActionResult> UpdatePlayerStat(string userId, UpdatePlayerBoxVM data)
        {
            var updateData = await _context.PlayerBoxings.FirstOrDefaultAsync(c => c.PlayerId == Guid.Parse(userId));

            updateData.BoxPower = data.BoxPower;
            updateData.BoxHighScore = data.BoxHighScore;
           
            await _context.SaveChangesAsync();
            return Ok(true);

        }




        [HttpGet("UserAllPower")]
        public async Task<IActionResult> MrOlimpiaPlayer(string username)
        {
            IQueryable<PlayerStat> query = _context.PlayerStats;
            IQueryable<Player> playerQuery = _context.Players;

            var sortedQuery = query.OrderByDescending(d => d.ALlPower);

            var result = await sortedQuery
                .Select(x => new
                {
                    UserId = x.PlayerId,
                    UserName = _context.Players.Where(p => p.Id == x.PlayerId).Select(p => p.UserName).FirstOrDefault(),
                    ALlPower = x.ALlPower
                })
                .ToListAsync();

            var newresult = result.Select((x, i) => new
            {
                Rank = i + 1,
                x.UserId,
                x.UserName,
                x.ALlPower
            })
            .ToList();
            var userTop100 = newresult.Take(100).ToList();

            Player? currentUser = await playerQuery.FirstOrDefaultAsync(z => z.UserName == username);

            var userRank = newresult.FirstOrDefault(x => x.UserId == currentUser.Id)?.Rank;

            return Ok(new { GeneralRank = userTop100, GeneralRankUser = userRank });
        }






        [HttpGet("mrOlimpiaWinerList")]
        public async Task<IActionResult> MrOlimpiaWinners(string userName)
        {
            IQueryable<PlayerStat> query = _context.PlayerStats;

            query = query.OrderByDescending(d => d.OlimpiaWin);

            var result = await query
                .Select(x => new { UserName = _context.Players.Where(p => p.Id == x.PlayerId).Select(p => p.UserName).FirstOrDefault(), OlimpiaWin = x.OlimpiaWin })
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


        [HttpGet("BoxList")]
        public async Task<IActionResult> BoxingRank(string userName)
        {
            IQueryable<PlayerBoxing> query = _context.PlayerBoxings;

            query = query.OrderByDescending(d => d.BoxHighScore);

            var result = await query
                .Select(x => new { UserName = _context.Players.Where(p => p.Id == x.PlayerId).Select(p => p.UserName).FirstOrDefault(), BoxHighScore = x.BoxHighScore })
                .ToListAsync();

            // Add the rank property in-memory
            var newResult = result.Select((x, i) => new
            {
                Rank = i + 1,
                x.UserName,
                x.BoxHighScore
            })
             .ToList();
            var topTen = newResult.Take(10);
            var currentUser = newResult.Where(z => z.UserName == userName);

            return Ok(new { BoxingRankTen = topTen, RankUser = currentUser });

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
