using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMS.Actions
{
    public class ProcessNotificationsAction
    {
        private readonly IConfiguration _configuration;
        private readonly NotificationRepository _notificationRepository;
        private readonly TeamsHelper _teamsHelper;
        private readonly ILogger<ProcessNotificationsAction> _logger;

        public ProcessNotificationsAction(IConfiguration configuration, NotificationRepository notificationRepository, TeamsHelper teamsHelper, ILogger<ProcessNotificationsAction> logger)
        {
            _configuration = configuration;
            _notificationRepository = notificationRepository;
            _teamsHelper = teamsHelper;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var webhookUrl = _configuration["TeamsWebhookUrl"];
            var teamsHelper = new TeamsHelper(webhookUrl);

            try
            {
                var (successCount, failCount) = await _notificationRepository.GetNotificationCountsAsync();
                await _teamsHelper.SendDailyReportAsync(successCount, failCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing notifications.");
            }
        }
    }

}
