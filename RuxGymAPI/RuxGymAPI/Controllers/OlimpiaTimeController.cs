using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using RuxGymAPI.VMModels;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OlimpiaTimeController : ControllerBase
    {
        
        private readonly RuxGymDBcontext _context;
        private List<OlimpiaPlayer> olimpiaPlayers;
        private List<PlayerStat> statData;


        public OlimpiaTimeController(RuxGymDBcontext context)
        {

            
            _context = context;
        }

        [HttpGet]
        public  IActionResult GetAsync()
        {
            
            string value =  TimeUntilNextTenMinutes().ToString();
            return Ok(value);
        }

        [HttpPut("Admin")]
        public async Task<IActionResult> OlimpiaRewards(string key)
        {
            if (key.Equals("RuxMilliondolar"))
            {
                await AddOlimpiaPlayer();
                var weeks = await _context.OlimpiaWeeks.FindAsync(1);
                weeks.Week += 1;
                if (olimpiaPlayers != null)
                {
                    int a = 1;

                    foreach (var item in olimpiaPlayers)
                    {

                        var userStats = await _context.PlayerStats.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(item.userId));
                        if (a == 1)
                        {
                            userStats.ProteinItem += 3;
                            userStats.OlimpiaWin += 1;
                            userStats.PlayerSpinCount += 50;
                            userStats.PlayerDiamond += 20;

                        }
                        else if (a == 2)
                        {
                            userStats.ProteinItem += 2;
                            userStats.PlayerSpinCount += 30;
                            userStats.PlayerDiamond += 10;

                        }
                        else if (a == 3)
                        {
                            userStats.ProteinItem += 1;
                            userStats.PlayerSpinCount += 10;
                            userStats.PlayerDiamond += 5;

                        }
                        else if (a == 4)
                        {

                            userStats.PlayerCash += 200000;

                        }
                        else if (a == 5)
                        {
                            userStats.PlayerCash += 160000;

                        }
                        else if (a == 6)
                        {
                            userStats.PlayerCash += 130000;

                        }
                        else if (a == 7)
                        {
                            userStats.PlayerCash += 100000;

                        }
                        else if (a == 8)
                        {
                            userStats.PlayerCash += 70000;

                        }
                        else if (a == 9)
                        {
                            userStats.PlayerCash += 50000;
                        }
                        else if (a == 10)
                        {
                            userStats.PlayerCash += 30000;
                        }

                        userStats.IsOlimpia = false;
                        await _context.SaveChangesAsync();
                        a++;
                    }
                    
                }
                if (weeks.Week >= 5)
                {
                    var userlist = await _context.PlayerStats.ToListAsync();
                    foreach (var user in userlist)
                    {
                        user.ALlPower = 50;
                        user.ArmPower = 10;
                        user.ChestPower = 10;
                        user.LegPower = 10;
                        user.SixpackPower = 10;
                        user.BackPower = 10;
                        await _context.SaveChangesAsync();

                    }
                    weeks.Week = 1;

                    await _context.SaveChangesAsync();
                    return Ok(true);
                }
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }

        }

        [HttpGet("Week")]
        public async Task<IActionResult> GetWeek()
        {
            var value = await _context.OlimpiaWeeks.FindAsync(1);
            return Ok(value.Week);
        }



        static  TimeSpan TimeUntilNextTenMinutes()
        {
            DateTime now = DateTime.Now; // Şu anki zamanı al
            DateTime nextHour = now.Date.AddHours(now.Hour + 1).AddMinutes(0); // Bir sonraki saat için DateTime oluştur
            return nextHour - now; // Geriye kalan süreyi hesapla
        }
        // every week
        static TimeSpan TimeUntilNextMondayAtMidnight()
        {
            DateTime now = DateTime.Now;
            DateTime nextMonday = now.AddDays((8 - (int)now.DayOfWeek) % 7).Date;
            DateTime mondayMidnight = nextMonday.Date.Add(new TimeSpan(0, 0, 0));
            TimeSpan remainingTime = mondayMidnight - now;

            if (remainingTime < TimeSpan.Zero)
            {
                nextMonday = nextMonday.AddDays(7);
                mondayMidnight = nextMonday.Date.Add(new TimeSpan(0, 0, 0));
                remainingTime = mondayMidnight - now;
            }
            return remainingTime;
        }


        private async Task UploadReward()
        {
            var currentUrl = "http://35.195.232.253/gymapi/api/OlimpiaTime/Admin?key=RuxMilliondolar";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsync(currentUrl, null);
                var responseContent = await response.Content.ReadAsStringAsync();

            }

        }
        private async Task AddOlimpiaPlayer()
        {
            var currentUrl = "http://35.195.232.253/gymapi/api/Stats/Joiners";
            olimpiaPlayers = new List<OlimpiaPlayer>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(currentUrl);
                var responseContent = await response.Content.ReadAsStringAsync();
                olimpiaPlayers = JsonConvert.DeserializeObject<List<OlimpiaPlayer>>(responseContent);

            }

        }
    }
}
