

namespace DAMS.DatabaseModel.ETMP.SYNC.DB.Models
{
    public partial class Job
    {
        public Job()
        {
         
            JobHistories = new HashSet<JobHistory>();
        }

        public int JobId { get; set; }
        public int SourceSystemId { get; set; }
        public string JobName { get; set; } = null!;
        public string? Description { get; set; }
        public string? JobSchedule { get; set; }
        public bool IsFullSync { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DependencyJobId { get; set; }

        
        public virtual ICollection<JobHistory> JobHistories { get; set; }
    }
}
