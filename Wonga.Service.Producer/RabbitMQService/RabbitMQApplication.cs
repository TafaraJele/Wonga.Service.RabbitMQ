using RabbitMQ.Client;
using Serilog;
using System;
using System.Text;
using Wonga.Service.Producer.Abstractions;
using Wonga.Service.Producer.Models;

namespace Wonga.Service.Producer.RabbitMQService
{
    public class RabbitMQApplication
    {
        private readonly ApplicationSettings _settings;
  
        public RabbitMQApplication(Message message, ApplicationSettings settings)
        {
            Message = message;
            _settings = settings;
           
        }
        public Message Message { get; set; }
        public void PublishMessage()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_settings.RabbitMQUrl)
            };
            Log.Information($"Logging....Creating connection to endpoint {_settings.RabbitMQUrl}");

            try
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(_settings.RoutingKey,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                Message.MessageText = $"Hello my name is, {Message.Name}";

                var body = Encoding.UTF8.GetBytes(Message.MessageText);

                Log.Information($"Publishing message text ....{Message.MessageText}");
                channel.BasicPublish(_settings.Exchange, _settings.RoutingKey, null, body);
                
            }
            catch(Exception e)
            {
                Log.Error($"Logging....Error creating connection to {_settings.RabbitMQUrl} Exception {e}");
                Console.WriteLine($"Error creating connection to {_settings.RabbitMQUrl}");
            }           
           
        }
    }

}
