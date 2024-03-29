using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using ShaRide.Application.Contexts;
using ShaRide.Application.Managers;
using ShaRide.Application.Seeds;

namespace ShaRide.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.PostgreSQL(
                    connectionString: config.GetConnectionString("DefaultConnection"),
                    tableName: "logs",
                    restrictedToMinimumLevel:LogEventLevel.Information,
                    needAutoCreateTable:true
                )
                .WriteTo.Debug(outputTemplate: DateTime.Now.ToString(CultureInfo.InvariantCulture))
                .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    Log.Information("Seeding Default Data Started");

                    var userManager = services.GetRequiredService<UserManager>();
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    await DefaultCounters.SeedAsync(dbContext);
                    await DefaultRoles.SeedAsync(userManager);
                    await DefaultAdminUsers.SeedAsync(userManager);
                    await DefaultSeats.SeedAsync(dbContext);
                    await CarBrandModelSeed.SeedAsync(dbContext);

                    Log.Information("Finished Seeding Default Data");
                    Log.Information("Application Starting");
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "An error occurred seeding the DB");
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() //Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("https://localhost:4242", "http://localhost:4243");
                });
    }
}