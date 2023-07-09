using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Models;

namespace RuxGymAPI.Context
{
    public class RuxGymDBcontext : DbContext
    {
        public RuxGymDBcontext(DbContextOptions<RuxGymDBcontext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }
        public DbSet<PlayerGymItem> PlayerGymItems { get; set; }
        public DbSet<PasswordCode> PasswordCodes { get; set; }
        public DbSet<PlayerEnergy> PlayerEnergies { get; set; }
        public DbSet<PlayerSpinTime> SpinDateTimes { get; set; }
        public DbSet<PlayerPremium> PlayerPremia { get; set; }
        public DbSet<OlimpiaWeek> OlimpiaWeeks { get; set; }
        public DbSet<PlayerBoxing> PlayerBoxings { get; set; }
        public DbSet<GameVersion> GameVersions { get; set; }

        


    }
}
