using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Skoruba.Duende.IdentityServer.Shared.Configuration.Helpers;

namespace TestDatabase
{
    internal class Program
    {
        //const string ConnectionString  = "data source=172.18.5.113,14330;initial catalog=PUMA;User Id=sa;Password=G@sFl0wDev;MultipleActiveResultSets=true";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Applying Docker configuration.");
            var configuration = GetConfiguration(args);
            DockerHelpers.ApplyDockerConfiguration(configuration);
            Console.WriteLine("Docker configuration applied.");

            await PingHosts();
            Console.WriteLine("----------------------------");
            await TestDatabaseConnection(configuration);

            Console.WriteLine($"Press a key to exit.");
            Console.Read();
        }

        private static async Task PingHosts()
        {
            foreach (var hostname in new[] {"dbskoruba", "sts.skoruba.local"})
            {
                Console.WriteLine($"Ping host  : '{hostname}'.");
                const int timeout = 10000;
                Ping ping = new Ping();
                try
                {
                    PingReply pingreply = ping.Send(hostname, timeout);
                    if (pingreply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("Address: {0}", pingreply.Address);
                        Console.WriteLine("status: {0}", pingreply.Status);
                        Console.WriteLine("Round trip time: {0}", pingreply.RoundtripTime);
                    }
                    else
                    {
                        Console.WriteLine("Ping FAILED reply status : {0}", pingreply.Status);

                    }
                }
                catch (PingException ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine($"Pinged host  : '{hostname}'.");
            }
        }

        private static async Task TestDatabaseConnection(IConfiguration configuration)
        {
            var cs = configuration.GetConnectionString("ConfigurationDbConnection");
            Console.WriteLine($"Found connection string in config : '{cs}'.");
            
            //foreach (var port in new[]{14330, 1433, 7900})
            {
                var builder = new SqlConnectionStringBuilder(cs)
                {
                    ConnectTimeout = 5,
                    //DataSource = $"sql2019skoruba,{port}"
                };
                cs = builder.ConnectionString;
                Console.WriteLine($"Connecting to database '{cs}'.");
                var conn = new SqlConnection(cs);
                try
                {
                    Console.WriteLine($"Sql connection created, opening.");

                    await conn.OpenAsync();
                    Console.WriteLine($"Sql connection succeeded.");
                    await conn.CloseAsync();
                    Console.WriteLine($"Sql connection closed.");
                    return;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
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
