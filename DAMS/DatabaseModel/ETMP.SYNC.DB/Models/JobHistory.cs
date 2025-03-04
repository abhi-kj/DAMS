
namespace DAMS.DatabaseModel.ETMP.SYNC.DB.Models
{
    public  class JobHistory
    {
      

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
   
    }
}
