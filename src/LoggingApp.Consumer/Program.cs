using LoggingApp.Consumer.Database;
using LoggingApp.Consumer.Services;

namespace LoggingApp.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<LogService>();
                    services.AddTransient<DatabaseContext>();
                })
                .Build();

            host.Run();
        }
    }
}