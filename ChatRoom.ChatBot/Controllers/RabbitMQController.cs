using ChatRoom.Application.Models;
using ChatRoom.ChatBot.Controllers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChatRoom.ChatBot.Configurations {
    public static class RabbitMQController {

        private static ConnectionFactory? Factory { get; set; }
        private static IConnection? Connection { get; set; }
        private static IModel? Channel { get; set; }
        private static EventingBasicConsumer? Consumer { get; set; }

        private static string? Queue { get; set; }
        private static string? HostName { get; set; }

        public static void Configure(IConfiguration configuration) {

            HostName = configuration.GetValue<string>("HostName");
            Queue = configuration.GetValue<string>("Queue");

            Console.WriteLine($"{DateTime.Now} - Connecting to {Queue}");

            Factory = new ConnectionFactory { HostName = HostName };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();

            Channel.QueueDeclare(queue: Queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            Console.WriteLine($"{DateTime.Now} - ChatBot connected to the channel {Queue}");

            Consumer = new EventingBasicConsumer(Channel);

            Consumer.Received += ReceivedMessageAsync;

            Channel.BasicConsume(queue: Queue,
                     autoAck: true,
                     consumer: Consumer);

        }

        public static async void ReceivedMessageAsync(object? model, BasicDeliverEventArgs eventArgs) {

            try {

                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<RabbitMessageDTO>(json);
                var messageIsCommand = message.Message.StartsWith("/");

                if (!messageIsCommand)
                    return;


                Console.WriteLine($"{DateTime.Now} - User '{message.UserName}' in room '{message.RoomName}' - {message.Message}");

                if (message.Message.StartsWith("/stock=")) {

                    var commandResult = await CommandController.StockCommand(message);

                    var resultDTO = new RabbitMessageDTO() {
                        Message = commandResult,
                        RoomName = message.RoomName,
                        UserName = "ChatBot"
                    };

                    var bodyJson = JsonConvert.SerializeObject(resultDTO);
                    var bodyResult = Encoding.UTF8.GetBytes(bodyJson);

                    Channel.BasicPublish(exchange: string.Empty,
                    routingKey: $"{Queue}return",
                    basicProperties: null,
                    body: bodyResult);

                }


            } catch (Exception error) {
                Console.WriteLine($"{DateTime.Now} - Error -> ReceivedMessage : {error.Message} ");
            }

        }



    }
}
