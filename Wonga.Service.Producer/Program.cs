using Newtonsoft.Json;
using Serilog;
using System;
using Wonga.Service.Producer.Abstractions;
using Wonga.Service.Producer.Models;
using Wonga.Service.Producer.RabbitMQService;

namespace Wonga.Service.Producer
{
    class Program
    {      
        static void Main()
        {     
            //Get Application settings from appsetttings file
            ApplicationSettings applicationSettings = new ApplicationSettings();
            ApplicationSettings settings = applicationSettings.GetApplicationSettings();
            string settingsData = JsonConvert.SerializeObject(settings);
            Log.Information($"Logging ....Application settings from appsettings.json fle {settingsData}");

            //Read name from Console 
            Console.WriteLine("Please input your name");
            Console.WriteLine("----------------------");
            Message message = new Message()
            {
                Name = Console.ReadLine()            
              
            };
            
            //Publish message to RabbitMQ if name is supplied
            if (!string.IsNullOrEmpty(message.Name))
            {
                Log.Information($"Logging ....user has input name {message.Name}");

                RabbitMQApplication rabbitMQService = new RabbitMQApplication(message, settings);
                rabbitMQService.PublishMessage();

            }
            else
            {                
                Console.WriteLine("Name is required");
                Program.Main();
            }
            Console.ReadLine();
            Log.CloseAndFlush();

        }

    }
}
