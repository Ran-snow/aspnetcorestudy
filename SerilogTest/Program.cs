using Serilog;
using Serilog.Context;

const string OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} <{ThreadId}> [{Level:u3}] {Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithThreadId()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
    .WriteTo.File("logs/app.txt"
        , rollingInterval: RollingInterval.Day
        , outputTemplate : OUTPUT_TEMPLATE)
    .CreateLogger();

//var sensorInput = new { Latitude = 25, Longitude = 134 };
//Log.Information("Processing {@SensorInput}", sensorInput);

Log.Information("No contextual properties");

using (LogContext.PushProperty("A", 1))
{
    Log.Information("Carries property A = {A}");

    using (LogContext.PushProperty("A", 2))
    using (LogContext.PushProperty("B", 1))
    {
        Log.Information("Carries A = {A} and B = {B}");
    }

    Log.Information("Carries property A = {A}, again");
}

//var count = 456;
//Log.Information("Retrieved {Count} records", count);

//var fruit = new Dictionary<string, int> { { "Apple", 1 }, { "Pear", 5 } };
//Log.Information("In my bowl I have {Fruit}", fruit);

//int a = 10, b = 0;
//Log.Debug("Dividing {A} by {B}", a, b);
//Log.Information("Hello, Serilog!");
//Log.Error("Hello, Serilog!");

Log.CloseAndFlush();
Console.WriteLine("Hello, World!");