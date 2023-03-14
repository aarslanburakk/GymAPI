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
    }
}
