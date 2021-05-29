using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;


namespace Wonga.Service.Consumer.Abstractions
{
    public class ApplicationSettings
    {
        IConfiguration _iconfiguration;
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string RabbitMQUrl { get; set; }

        public ApplicationSettings GetApplicationSettings()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();

            ApplicationSettings applicationSettings = new ApplicationSettings()
            {
                Exchange = _iconfiguration.GetSection("ApplicationSettings:Exchange").Value,
                RoutingKey = _iconfiguration.GetSection("ApplicationSettings:RoutingKey").Value,
                RabbitMQUrl = _iconfiguration.GetSection("ApplicationSettings:RabbitMQUrl").Value,

            };
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_iconfiguration)
                .CreateLogger();

            return applicationSettings;
        }
    }
}
