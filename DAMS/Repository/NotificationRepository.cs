
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

        public NotificationRepository(INTDbContext context)
        {
            _context = context;
        }

        public async Task<(int SuccessCount, int FailCount)> GetNotificationCountsAsync()
        {
            var oneDayAgo = DateTime.UtcNow.AddDays(-1);
            var notifications = await _context.Notifications
                .Where(n => n.CreatedDate >= oneDayAgo)
                .ToListAsync();

            var successCount = notifications.Count(n => n.IsSent);
            var failCount = notifications.Count(n => !n.IsSent);

            return (successCount, failCount);
        }
    }
}
