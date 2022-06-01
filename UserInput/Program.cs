
using ElasticEmailAPI;
using ElasticEmailAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserInput;
using UserInput.ConsolePrompter;

static void ConfigureServices(IServiceCollection serviceCollection)
{
    serviceCollection.AddLogging(builder =>
    {
        builder.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.IncludeScopes = false;
            options.TimestampFormat = "hh:mm:ss ";

        });
#if(!DEBUG)
        builder.SetMinimumLevel(LogLevel.Error);
#else
        builder.SetMinimumLevel(LogLevel.Trace);
#endif
    });

    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    serviceCollection.Configure<BasicServiceConfigruation>(configuration.GetSection("ElasticEmailSettings"));
    serviceCollection.AddSingleton<EmailService>();
    serviceCollection.AddSingleton<IConsoleManager, ConsoleManager>();
    serviceCollection.AddSingleton<UserInputManager>();

    serviceCollection.AddTransient<Application>();
}

var services = new ServiceCollection();
ConfigureServices(services);

using var serviceHost = services.BuildServiceProvider();

try
{
    await serviceHost.GetRequiredService<Application>().Run();
}
catch (InvalidOperationException e)
{
    Console.WriteLine("Application was unable to start!");
    Console.WriteLine(e.Message);
}