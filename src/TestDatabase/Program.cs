using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Skoruba.Duende.IdentityServer.Shared.Configuration.Helpers;

namespace TestDatabase
{
    internal class Program
    {
        const string ConnectionString  = "data source=192.168.117.177,14330;initial catalog=PUMA;User Id=sa;Password=G@sFl0wDev;MultipleActiveResultSets=true";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Applying Docker configuration.");
            var configuration = GetConfiguration(args);
            DockerHelpers.ApplyDockerConfiguration(configuration);
            Console.WriteLine("Docker configuration applied.");
            
            Console.WriteLine($"Connecting to database '{ConnectionString}'.");
            var conn = new SqlConnection(ConnectionString);
            Console.WriteLine($"Sql connection created, opening.");

            await conn.OpenAsync();
            Console.WriteLine($"Sql connection succeeded.");
            await conn.CloseAsync();
            Console.WriteLine($"Sql connection closed.");
            
            Console.WriteLine($"Press a key to exit.");
            Console.Read();
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                ;
           
            configurationBuilder.AddCommandLine(args);
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }
}
