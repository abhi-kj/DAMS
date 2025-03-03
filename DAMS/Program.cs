class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build())
            .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddUserSecrets<Program>();
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

                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddSerilog();
                    });
                });

            var host = builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var notificationRepository = services.GetRequiredService<NotificationRepository>();
                var teamsHelper = services.GetRequiredService<TeamsHelper>();

                try
                {
                    var (successCount, failCount) = await notificationRepository.GetNotificationCountsAsync();
                    await teamsHelper.SendDailyReportAsync(successCount, failCount);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while processing notifications.");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
