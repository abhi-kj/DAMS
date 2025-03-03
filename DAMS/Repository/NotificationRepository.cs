
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

        public async Task<(int SuccessCount, int FailCount)> GetNotificationCountsAsync()
        {
            try
            {
                int daysToCheck = _configuration.GetValue<int>("NotificationSettings:DaysToCheck");
                var dateThreshold = DateTime.UtcNow.AddDays(-daysToCheck);
                var notifications = await _context.Notification
                    .Where(n => n.CreatedDate >= dateThreshold)
                    .ToListAsync();

                var successCount = notifications.Count(n => n.IsSent);
                var failCount = notifications.Count(n => !n.IsSent);

                return (successCount, failCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting notification counts.");
                throw;
            }
        }
    }
}
