

namespace DAMS.DatabaseModel.ETMP.INT.DB.DBContext
{


    public class INTDbContext : DbContext
    {
        public INTDbContext(DbContextOptions<INTDbContext> options) : base(options) { }

        public DbSet<Notification> Notification { get; set; }
        public DbSet<UserNotificationExclusion> UserNotificationExclusion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Name=ETMPINTDBConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserNotificationExclusion>(entity =>
        {
            entity.ToTable("UserNotificationExclusion");

            entity.Property(e => e.UserNotificationExclusionId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserWwid)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("UserWWID");


        });
        }

    }

}
