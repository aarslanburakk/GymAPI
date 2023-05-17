using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Models;

namespace RuxGymAPI.Context
{
    public class RuxGymDBcontext : DbContext
    {
        public RuxGymDBcontext()
        {
        }

        public RuxGymDBcontext(DbContextOptions<RuxGymDBcontext> options) : base(options)
        {

        }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }
        public DbSet<PlayerGymItem> PlayerGymItems { get; set; }
        public DbSet<PasswordCode> PasswordCodes { get; set; }
        public DbSet<PlayerEnergy> PlayerEnergies { get; set; }
        public DbSet<SpinDateTime> SpinDateTimes { get; set; }
        public DbSet<PlayerPremium> PlayerPremia { get; set; }
        public DbSet<OlimpiaWeek> OlimpiaWeeks { get; set; }
       
        
        
    }
}
