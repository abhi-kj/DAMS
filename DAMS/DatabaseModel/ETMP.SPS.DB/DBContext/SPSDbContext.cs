namespace DAMS.DatabaseModel.ETMP.SPS.DB.DBContext
{
    public class SPSDbContext : DbContext
    {
        public SPSDbContext(DbContextOptions<SPSDbContext> options) : base(options) { }

        public DbSet<Logs>  Logs { get; set; }
    }
}
