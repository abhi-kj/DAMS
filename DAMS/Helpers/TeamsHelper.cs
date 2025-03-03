
namespace DAMS.Helpers
{
    public class TeamsHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _webhookUrl;

        public TeamsHelper(HttpClient httpClient, string webhookUrl)
        {
            _httpClient = httpClient;
            _webhookUrl = webhookUrl;
        }
        
        public TeamsHelper( string webhookUrl)
        {
            
            _webhookUrl = webhookUrl;
        }

      

        public async Task SendMessageAsync(string message)
        {
            using var _httpClient = new HttpClient();
            var payload = new { text = message };
            await _httpClient.PostAsJsonAsync(_webhookUrl, payload);
        }

        public async Task SendMessageSuccessAsync(string repository, string branch, string commit, string runId, string serverUrl)
        {
            using var _httpClient = new HttpClient();
            var messageCard = new
            {
                @type = "MessageCard",
                @context = "https://schema.org/extensions",
                summary = "Pipeline Success!",
                themeColor = "00ff00",
                title = $"{repository} pipeline success 🥳!",
                sections = new[]
                {
                new
                {
                    facts = new[]
                    {
                        new { name = "Repository:", value = repository },
                        new { name = "Branch:", value = branch },
                        new { name = "Commit:", value = commit }
                    }
                }
            },
                potentialAction = new[]
                {
                new
                {
                    @type = "OpenUri",
                    name = "View on GitHub",
                    targets = new[]
                    {
                        new { os = "default", uri = $"{serverUrl}/{repository}/actions/runs/{runId}" }
                    }
                }
            }
            };

            await _httpClient.PostAsJsonAsync(_webhookUrl, messageCard);
        }


        public async Task SendDailyReportAsync(int successCount, int failCount)
        {
            using var _httpClient = new HttpClient();
            var currentDate = DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd HH:mm:ss"); // IST time
            var messageCard = new
            {
                @type = "MessageCard",
                @context = "https://schema.org/extensions",
                summary = "Daily Mail Report",
                themeColor = "0078D7",
                title = $"Update on {currentDate}",
                sections = new[]
                {
                    new
                    {
                        activityTitle = "Daily Mail Report of ETMP Production",
                        facts = new[]
                        {
                            new { name = "Successful Messages:", value = $"{successCount} messages were sent successfully." },
                            new { name = "Failed Messages:", value = $"{failCount} messages failed to send." }
                        }
                    }
                }
            };

            await _httpClient.PostAsJsonAsync(_webhookUrl, messageCard);
        }

    }
}
