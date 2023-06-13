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
       
        public OlimpiaTimeController(RuxGymDBcontext context)
        {


            _context = context;
        }

        

        [HttpGet]
        public IActionResult GetAsync()
        {

            string value = TimeUntilNextMondayAtMidnight().ToString();
            return Ok(value);
        }

        [HttpPut("Admin")]
        public async Task<IActionResult> OlimpiaRewards(string key)
        {
            var joiners = _context.PlayerStats.OrderByDescending(x => x.ALlPower).Where(c => c.IsOlimpia.Equals(true));
            if (key.Equals("RuxMilliondolar"))
            {

                var weeks = await _context.OlimpiaWeeks.FindAsync(1);
                weeks.Week += 1;
                if (joiners != null)
                {
                    int a = 1;

                    foreach (var item in joiners)
                    {


                        if (a == 1)
                        {
                            item.ProteinItem += 3;
                            item.OlimpiaWin += 1;
                            item.PlayerSpinCount += 50;
                            item.PlayerDiamond += 20;

                        }
                        else if (a == 2)
                        {
                            item.ProteinItem += 2;
                            item.PlayerSpinCount += 30;
                            item.PlayerDiamond += 10;

                        }
                        else if (a == 3)
                        {
                            item.ProteinItem += 1;
                            item.PlayerSpinCount += 10;
                            item.PlayerDiamond += 5;

                        }
                        else if (a == 4)
                        {

                            item.PlayerCash += 200000;

                        }
                        else if (a == 5)
                        {
                            item.PlayerCash += 160000;

                        }
                        else if (a == 6)
                        {
                            item.PlayerCash += 130000;

                        }
                        else if (a == 7)
                        {
                            item.PlayerCash += 100000;

                        }
                        else if (a == 8)
                        {
                            item.PlayerCash += 70000;

                        }
                        else if (a == 9)
                        {
                            item.PlayerCash += 50000;
                        }
                        else if (a == 10)
                        {
                            item.PlayerCash += 30000;
                        }

                        item.IsOlimpia = false;

                        a++;
                    }

                }
                var userlist = await _context.PlayerStats.ToListAsync();
                if (weeks.Week >= 5)
                {

                    foreach (var user in userlist)
                    {
                        user.ALlPower = 50;
                        user.ArmPower = 10;
                        user.ChestPower = 10;
                        user.LegPower = 10;
                        user.SixpackPower = 10;
                        user.BackPower = 10;

                    }
                    weeks.Week = 1;
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



        static TimeSpan TimeUntilNextTenMinutes()
        {
            DateTime now = DateTime.Now; // Şu anki zamanı al
            DateTime nextHour = now.Date.AddHours(now.Hour + 1); // Bir sonraki saat için DateTime oluştur
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

    }
}

