using Serilog;
using Serilog.Events;

const string OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} <{ThreadId}> [{Level:u3}] {Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
    .Enrich.WithThreadId()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
    .WriteTo.File("logs/app.txt"
        , rollingInterval: RollingInterval.Day
        , outputTemplate: OUTPUT_TEMPLATE)
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication
        .CreateBuilder(args);

    builder.Host.UseSerilog(Log.Logger, dispose: true);

    // Add services to the container.

    builder.Services.AddControllers();

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseAuthorization();
    app.UseHttpLogging();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}