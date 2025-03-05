
using DAMS.DTO;
using Newtonsoft.Json;
using System.Text;

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

        public TeamsHelper(string webhookUrl)
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


        public async Task SendDailyReportAsyncChannel(ReportData reportData)
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
                            new { name = "Successful Mails:", value = $"<span style='color:green'><b>{reportData.MailReportData.MailSuccessCount}</b></span> messages were sent successfully." },
                            new { name = "Failed Mails:", value = reportData.MailReportData.MailFailCount > 0 ? $"<span style='color:red'><b>{reportData.MailReportData.MailFailCount}</b></span> messages failed to send." : $"{reportData.MailReportData.MailFailCount} messages failed to send." },
                            new { name = "Skipped Mails:", value = $"<b>{reportData.MailReportData.SkipCount}</b> messages were skipped." }
                        }
                    },
                    new
                    {
                        activityTitle = "<span style='color:blue'>________________________</span>",

                        facts = new[]
                        {
                            new { name = "", value = "" }
                        }
                    },
                    new
                    {
                        activityTitle = "Daily Sync Report of ETMP Production",
                        facts = new[]
                        {
                            new { name = "Successful Job Syncs:", value = $"<span style='color:green'><b>{reportData.SyncReportData.SyncSuccessCount}</b></span> syncs were successful." },
                            new { name = "Failed Job Syncs:", value = reportData.SyncReportData.SyncFailCount > 0 ? $"<span style='color:red'><b>{reportData.SyncReportData.SyncFailCount}</b></span> syncs failed." : $"{reportData.SyncReportData.SyncFailCount} syncs failed." }
                        }
                    }
                }
            };

            await _httpClient.PostAsJsonAsync(_webhookUrl, messageCard);
        }

        public async Task SendDailyReportAsync(ReportData reportData)
        {
            using var _httpClient = new HttpClient();
            var currentDate = DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd HH:mm:ss"); // IST time
            //var x = "{\r\n  \"type\": \"message\",\r\n  \"attachments\": [\r\n    {\r\n      \"contentType\": \"application/vnd.microsoft.card.adaptive\",\r\n      \"content\": {\r\n        \"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\r\n        \"type\": \"AdaptiveCard\",\r\n        \"version\": \"1.2\",\r\n        \"body\": [\r\n          {\r\n            \"type\": \"TextBlock\",\r\n            \"text\": \"Hello, this is a message sent via Power Automate!\",\r\n            \"wrap\": true\r\n          }\r\n        ],\r\n        \"actions\": [\r\n          {\r\n            \"type\": \"Action.OpenUrl\",\r\n            \"title\": \"Learn More\",\r\n            \"url\": \"https://adaptivecards.io\"\r\n          }\r\n        ]\r\n      }\r\n    }\r\n  ]\r\n}";
            var messageCard = new
            {
                type = "message",
                attachments = new[]
                {
                    new
                    {
                        contentType = "application/vnd.microsoft.card.adaptive",
                        content = new
                        {
                            xschema = "http://adaptivecards.io/schemas/adaptive-card.json",
                            type = "AdaptiveCard",
                            version = "1.2",
                            body = new[]
                            {
                                new
                                {
                                    type = "TextBlock",
                                    text = $"Update on {currentDate}",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = "Daily Mail Report of ETMP Production",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = $"Successful Mails: {reportData.MailReportData.MailSuccessCount} messages were sent successfully.",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = reportData.MailReportData.MailFailCount > 0 ? $"Failed Mails: {reportData.MailReportData.MailFailCount} messages failed to send." : $"Failed Mails: {reportData.MailReportData.MailFailCount} messages failed to send.",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = $"Skipped Mails: {reportData.MailReportData.SkipCount} messages were skipped.",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = "________________________",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = "Daily Sync Report of ETMP Production",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = $"Successful Job Syncs: {reportData.SyncReportData.SyncSuccessCount} syncs were successful.",
                                    wrap = true
                                },
                                new
                                {
                                    type = "TextBlock",
                                    text = reportData.SyncReportData.SyncFailCount > 0 ? $"Failed Job Syncs: {reportData.SyncReportData.SyncFailCount} syncs failed." : $"Failed Job Syncs: {reportData.SyncReportData.SyncFailCount} syncs failed.",
                                    wrap = true
                                }
                            }
                        }
                    }
                }
            };

            var messageCardJson = JsonConvert.SerializeObject(messageCard);
            messageCardJson = messageCardJson.Replace("xschema", "$schema");
            await _httpClient.PostAsync(_webhookUrl, new StringContent(messageCardJson, Encoding.UTF8, "application/json"));
        }





    }
}
