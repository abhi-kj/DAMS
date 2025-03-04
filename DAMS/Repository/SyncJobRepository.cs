

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
                var dt = _context.JobHistories.Where(w => w.CreatedDate >= startDate && w.CreatedDate < endDate)
                          .Join(_context.Jobs,
                              his => his.JobId,
                              job => job.JobId,
                              (his, job) => new
                              {
                                  history = his,
                                  job = job
                              })
                          .Join(_context.JobHistoryLogs,
                              h => h.history.JobHistoryId,
                              log => log.JobHistoryId,
                              (h, log) =>
                              new ETMPJobHistory
                              {
                                  JobTitle = h.job.JobName,
                                  JobID = h.history.JobId,
                                  StatusId = h.history.StatusId,
                                  JobHistoryID = h.history.JobHistoryId,
                                  HangfireJobID = h.history.HangfireJobId,
                                  JobStartDate = h.history.JobStartDate,
                                  Message = h.history.Message ?? log.LogDesc,
                                  CreatedBy = h.history.CreatedBy,
                                  CreatedDate = h.history.CreatedDate
                              }).ToList();

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
