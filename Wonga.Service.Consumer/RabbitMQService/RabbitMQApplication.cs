using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text;
using Wonga.Service.Consumer.Abstractions;

namespace Wonga.Service.Consumer.RabbitMQService
{
    public class RabbitMQApplication
    {
        private readonly ApplicationSettings _settings;
        public RabbitMQApplication(ApplicationSettings settings)
        {
            _settings = settings;

        }
        public void ConsumeMessage()
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

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    byte[] body = e.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);
                    ValidateMesssage(message);
                };

                channel.BasicConsume($"{_settings.RoutingKey}", true, consumer);
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Log.Error($"Logging....Error creating connection to {_settings.RabbitMQUrl}/n" +
                    $"Exception {e}");
            }
        }
        //message must be private. Method made public for Unit tests
        public bool ValidateMesssage(string message)
        {
            //Validate message, extract name and print new message if not empty
            if (!string.IsNullOrEmpty(message))
            {
                string name;
                string[] messageArray = message.Split(',');
                try { name = messageArray[1]; }
                catch(Exception e)
                {
                    Log.Information($"Wrong message format {message} exception {e}");
                    return false;
                }
                
                if (!string.IsNullOrEmpty(name))
                {
                    Log.Information($"Received message {message}");
                    Console.WriteLine($"Hello{name}, I am  your father!");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }

    }
}
