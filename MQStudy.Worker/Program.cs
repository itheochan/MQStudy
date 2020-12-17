using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MQStudy.Worker
{
    public class Program
    {
        private static ServiceProvider ServiceProvider;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                var env = hostingContext.HostingEnvironment;
                if (env.IsDevelopment())
                {
                    config.AddJsonFile("appsettings.Development.json");
                    LogMessage("Application lunch config£ºappsettings.Development.json");
                }
                else
                {
                    config.AddJsonFile("appsettings.json");
                    LogMessage("Application lunch config£ºappsettings.json");
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(hostContext.Configuration);
                services.AddRabbitMQClient(hostContext.Configuration);
                
                services.AddHostedService<Worker>();
                services.AddHostedService<RabbitMQConsumer>();

                ServiceProvider = services.BuildServiceProvider();
            });




            return host;
        }

        public static void LogMessage(string message)
        {
            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "applog.txt"));
            if (!file.Exists) using (var tmp = file.Create()) { }
            using FileStream fs = File.Open(file.FullName, FileMode.Append);
            using StreamWriter fw = new StreamWriter(fs);
            fw.WriteLine($"\n[{DateTimeOffset.Now}]\t{message}");
            fw.Flush();
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
