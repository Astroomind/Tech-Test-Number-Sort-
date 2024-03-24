using Microsoft.EntityFrameworkCore;

namespace Technical_Test.Models
{
    public class SortContext : DbContext
    {
        public DbSet<SortResult> SortResults { get; set; }

        public SortContext(DbContextOptions Options) : base(Options)
        {
            
        }
    }
}
