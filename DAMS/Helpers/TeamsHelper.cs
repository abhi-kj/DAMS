
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

        public async Task SendMessageAsync(string message)
        {
            var payload = new { text = message };
            await _httpClient.PostAsJsonAsync(_webhookUrl, payload);
        }

        public async Task SendMessageSuccessAsync(string repository, string branch, string commit, string runId, string serverUrl)
        {
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
    }

}
