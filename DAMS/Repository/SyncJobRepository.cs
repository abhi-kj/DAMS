

using DAMS.DatabaseModel.ETMP.SYNC.DB.DBContext;
using DAMS.DTO;
using System.Linq;

namespace DAMS.Repository
{

    public class SyncJobRepository
    {
        private readonly SyncDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SyncJobRepository> _logger;

        public SyncJobRepository(SyncDbContext context, IConfiguration configuration, ILogger<SyncJobRepository> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public SyncReportData GetJobReportData()
        {
            try
            {
                int daysToCheck = _configuration.GetValue<int>("NotificationSettings:DaysToCheck");
                var startDateTime = DateTime.UtcNow.AddDays(-daysToCheck);


                var jobs = GetJobHistoriesByDateRange(startDateTime, DateTime.UtcNow);

                var total = jobs.Count();
                var successCount = jobs.Count(n => n.StatusId == 2);


                return new SyncReportData() { SyncSuccessCount = successCount, SyncFailCount = total - successCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting job report data.");
                throw;
            }
        }



        public List<ETMPJobHistory> GetJobHistoriesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var dt = (from history in _context.JobHistories
                          where history.CreatedDate >= startDate && history.CreatedDate < endDate
                          join job in _context.Jobs on history.JobId equals job.JobId
                          join log in _context.JobHistoryLogs on history.JobHistoryId equals log.JobHistoryId
                          select new ETMPJobHistory
                          {
                            
                              JobID = history.JobId,
                              StatusId = history.StatusId,
                              JobHistoryID = history.JobHistoryId,
                           
                          }).AsNoTracking().ToList();

                return dt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GetJobHistoriesByDateRange.");
                throw;
            }

        }

    }
}
