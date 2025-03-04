
namespace DAMS.DatabaseModel.ETMP.SYNC.DB.Models
{
    public partial class JobHistory
    {
        public JobHistory()
        {
            JobHistoryLogs = new HashSet<JobHistoryLog>();
        }

        public Guid JobHistoryId { get; set; }
        public int JobId { get; set; }
        public DateTime JobStartDate { get; set; }
        public DateTime? JobEndDate { get; set; }
        public int StatusId { get; set; }
        public string? Message { get; set; }
        public long HangfireJobId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Job Job { get; set; } = null!;
        public virtual ICollection<JobHistoryLog> JobHistoryLogs { get; set; }


    }
}
