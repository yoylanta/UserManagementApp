using Serilog;
using Serilog.Events;

namespace UserManagementApp.Presentation.Module;

public static  class SerilogInitializer
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var logLevel = configuration.GetValue<string>("Logging:LogLevel:Default");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(Enum.Parse<LogEventLevel>(logLevel, true))
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        hostBuilder.UseSerilog(); 

        return hostBuilder;
    }
}