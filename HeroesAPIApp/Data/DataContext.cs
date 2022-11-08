using HeroesAPIApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HeroesAPIApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Hero> Heroes => Set<Hero>();
        //public DbSet<Rootobject> Rootobject => Set<Rootobject>();
    }
}
