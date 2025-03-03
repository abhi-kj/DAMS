

namespace DAMS.DatabaseModel.ETMP.INT.DB.DBContext
{
    

    public class INTDbContext : DbContext
    {
        public INTDbContext(DbContextOptions<INTDbContext> options) : base(options) { }

        public DbSet<Notification> Notification { get; set; }
    }

  

}
