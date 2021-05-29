using Newtonsoft.Json;
using Serilog;
using System;
using Wonga.Service.Consumer.Abstractions;
using Wonga.Service.Consumer.RabbitMQService;

namespace Wonga.Service.Consumer
{
    class Program
    {
        static void Main()
        {  
            //Get Application settings from appsetttings file
            ApplicationSettings applicationSettings = new ApplicationSettings();
            var settings = applicationSettings.GetApplicationSettings();

            string settingsData = JsonConvert.SerializeObject(settings);
            Log.Information($"Logging ....Application settings from appsettings.json file {settingsData}");

            //Consume message to RabbitMQ
            RabbitMQApplication rabbitMQService = new RabbitMQApplication(settings);
            rabbitMQService.ConsumeMessage();
            
        }
    }
}
