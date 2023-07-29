using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Controllers;
using RuxGymAPI.Models;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RuxGymAPI.Repository.Tournament
{
    public class EfTournament : ITournament
    {
        private readonly RuxGymDBcontext _context;
        private Timer _timer;
        private bool isTimerElapsed = true;

        public EfTournament(RuxGymDBcontext context)
        {
            _context = context;
        }

        public async Task OlimpiaReward(string key)
        {
            var joiners = await _context.PlayerStats.OrderByDescending(x => x.ALlPower).Where(c => c.IsOlimpia.Equals(true)).Take(5).ToListAsync();
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

                            item.PlayerCash += 80000;

                        }
                        else if (a == 5)
                        {
                            item.PlayerCash += 50000;

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


            }
            var boxData = await _context.Players.Include(x => x.PlayerStat).Include(z => z.PlayerBoxing).OrderByDescending(s => s.PlayerBoxing.BoxHighScore).Take(5).ToListAsync();
            int c = 1;
            foreach (var item in boxData)
            {

                if (c == 1)
                {
                    item.PlayerStat.PlayerDiamond += 50;
                    item.PlayerStat.PlayerGoldTicket += 3;
                    item.PlayerStat.PlayerSpinCount += 30;


                }
                else if (c == 2)
                {
                    item.PlayerStat.PlayerDiamond += 30;
                    item.PlayerStat.PlayerGoldTicket += 2;
                    item.PlayerStat.PlayerSpinCount += 15;
                }
                else if (c == 3)
                {
                    item.PlayerStat.PlayerDiamond += 15;
                    item.PlayerStat.PlayerGoldTicket += 1;
                    item.PlayerStat.PlayerSpinCount += 8;
                }
                else if (c == 4)
                {

                    item.PlayerStat.PlayerDiamond += 5;
                    item.PlayerStat.PlayerSpinCount += 5;

                }
                else if (c == 5)
                {
                    item.PlayerStat.PlayerSpinCount += 3;
                    item.PlayerStat.PlayerCash += 30000;
                }
                c++;


            }
            await _context.SaveChangesAsync();
        }

        private async Task TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //string apiUrl = "http://localhost:5210/api/OlimpiaTime/Admin?key=RuxMilliondolar"; // API'nin URL'i
            string apiUrl = "http://104.40.246.13/RuxGym/api/OlimpiaTime/Admin?key=RuxMilliondolar"; // API'nin URL'i

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PutAsync(apiUrl, null);


            }
            OlimpiaTimeController.isTimerElapsed = true;
            _timer.Stop();
            StartTime();
        }
        public void StartTime()
        {
            var date = TimeUntilNextMondayAtMidnight();
            _timer = new Timer(date.TotalMilliseconds);
            _timer.Elapsed += async (sender, e) => await TimerElapsed(sender, e);
            _timer.AutoReset = false;
            _timer.Start();
            OlimpiaTimeController.isTimerElapsed = false;
        }

        public TimeSpan TimeUntilNextMondayAtMidnight()
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

        public async Task<int> GetOlympiaWeek()
        {
            var value = await _context.OlimpiaWeeks.FindAsync(1);
            return value.Week;
        }


    }
}
