

class Program
{
    static async Task Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                var webhookUrl = context.Configuration["TeamsWebhookUrl"];

                services.AddDbContext<INTDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<NotificationRepository>();
               
                services.AddHttpClient<TeamsHelper>(client =>
                {
                    client.BaseAddress = new Uri(webhookUrl);
                });
            });

        var host = builder.Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var notificationRepository = services.GetRequiredService<NotificationRepository>();
            var teamsHelper = services.GetRequiredService<TeamsHelper>();

            var (successCount, failCount) = await notificationRepository.GetNotificationCountsAsync();
            var message = $"In the last 24 hours, there were {successCount} successful notifications and {failCount} failed notifications.";

            await teamsHelper.SendMessageAsync(message);
        }
    }
}
