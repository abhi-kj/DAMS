

namespace DAMS.DatabaseModel.ETMP.SYNC.DB.Models
{
    public partial class JobHistoryLog
    {
        public JobHistoryLog()
        {

        }

        public int JobHistoryLogId { get; set; }
        public Guid JobHistoryId { get; set; }
        public int? EtlsyncConfigId { get; set; }
        public string? TargetDatabaseName { get; set; }
        public string? TargetTableName { get; set; }
        public string? Action { get; set; }
        public int? AffectedRowCount { get; set; }
        public int? StatusId { get; set; }
        public DateTime LogDate { get; set; }
        public string? LogDesc { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public virtual JobHistory JobHistory { get; set; } = null!;


    }
}
