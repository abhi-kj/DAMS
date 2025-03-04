
using DAMS.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMS.Repository
{

    public class NotificationRepository
    {
        private readonly INTDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(INTDbContext context, IConfiguration configuration, ILogger<NotificationRepository> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<MailReportData> GetNotificationCountsAsync()
        {
            try
            {
                int daysToCheck = _configuration.GetValue<int>("NotificationSettings:DaysToCheck");
                var dateThreshold = DateTime.UtcNow.AddDays(-daysToCheck);
                var notifications = await _context.Notification
                    .Where(n => n.CreatedDate >= dateThreshold).AsNoTracking()
                    .ToListAsync();

                var successCount = notifications.Count(n => n.IsSent);
                var failedMails = notifications.Where(n => !n.IsSent).ToList();
                var failCount = failedMails.Count();
                var skipCount = GetSkipMailCount(failedMails);
                failCount = failCount - skipCount;

                return new MailReportData() { MailSuccessCount = successCount, MailFailCount = failCount, SkipCount = skipCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting notification counts.");
                throw;
            }
        }

        private int GetSkipMailCount(List<Notification> failedMails)
        {
            var notificationTemplateIds = failedMails.Select(n => n.NotificationTemplateId).ToList();

            var skipCount = _context.UserNotificationExclusion
                .Where(une => une.IsActive == true && notificationTemplateIds.Contains(une.NotificationTemplateId)).AsNoTracking()
                .Select(une => une.UserWwid)
                .Distinct()
                .Count();

            return skipCount;
        }

    }
}
