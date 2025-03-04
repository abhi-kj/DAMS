using DAMS.DatabaseModel.ETMP.SYNC.DB.DBContext;
using DAMS.DTO;

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
                    var webhookUrl = context.Configuration["TeamsWebhookUrl"];
                    var ETMPINTDBConnection = context.Configuration.GetConnectionString("ETMPINTDBConnection");

                    services.AddDbContext<INTDbContext>(options =>
                        options.UseSqlServer(ETMPINTDBConnection));


                    var ETMPSyncDBConnection = context.Configuration.GetConnectionString("ETMPSyncDBConnection");

                    services.AddDbContext<SyncDbContext>(options =>
                        options.UseSqlServer(ETMPSyncDBConnection));

                    services.AddScoped<NotificationRepository>();
                    services.AddScoped<SyncJobRepository>();

                    services.AddScoped<TeamsHelper>();

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
                var context = services.GetRequiredService<IConfiguration>();
                var webhookUrl = context["TeamsWebhookUrl"];
                var notificationRepository = services.GetRequiredService<NotificationRepository>();
                var syncJobRepository = services.GetRequiredService<SyncJobRepository>();
                var teamsHelper = new TeamsHelper(webhookUrl);

                try
                {
                    var sycReportData = syncJobRepository.GetJobReportData();
                    var emailReportData = await notificationRepository.GetNotificationCountsAsync();
                    await teamsHelper.SendDailyReportAsync(new ReportData()
                    {
                        MailReportData = emailReportData,
                        SyncReportData = sycReportData
                    });
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
            Log.Fatal(ex, "Host terminated unexpectedly" + Environment.NewLine + ex.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
