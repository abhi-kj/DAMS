

using DAMS.DatabaseModel.ETMP.SYNC.DB.Models;

namespace DAMS.DatabaseModel.ETMP.SYNC.DB.DBContext
{


    public class SyncDbContext : DbContext
    {
        public SyncDbContext(DbContextOptions<SyncDbContext> options) : base(options) { }


        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<JobHistory> JobHistories { get; set; } = null!;
        public virtual DbSet<JobHistoryLog> JobHistoryLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.JobName).HasMaxLength(100);

                entity.Property(e => e.JobSchedule).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

               
            });

            modelBuilder.Entity<JobHistory>(entity =>
            {
                entity.ToTable("JobHistory");

                entity.Property(e => e.JobHistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("JobHistoryID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.JobEndDate).HasColumnType("datetime");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobStartDate).HasColumnType("datetime");

                entity.Property(e => e.Message).HasMaxLength(1000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

             
            });

            modelBuilder.Entity<JobHistoryLog>(entity =>
            {
                entity.ToTable("JobHistoryLog");

                entity.Property(e => e.JobHistoryLogId).HasColumnName("JobHistoryLogID");

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EtlsyncConfigId).HasColumnName("ETLSyncConfigID");

                entity.Property(e => e.JobHistoryId).HasColumnName("JobHistoryID");

                entity.Property(e => e.TargetDatabaseName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TargetTableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            });
        }
    }



}
