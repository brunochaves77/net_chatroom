using ChatRoom.Application.Models;
using ChatRoom.WebApplication.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatRoom.WebApplication.ApplicationService {
    public static class RabbitMQApplicationService {

        public static string? HostName { get; set; }
        public static string? Queue { get; set; }

        private static ConnectionFactory? Factory { get; set; }
        private static IConnection? Connection { get; set; }
        private static RabbitMQ.Client.IModel? Channel { get; set; }
        private static EventingBasicConsumer? Consumer { get; set; }

        public static IServiceProvider Provider { private get; set; }



        public static void ConfigureRabbitMQ(this WebApplicationBuilder builder) {

            HostName = builder.Configuration.GetValue<string>("RabbitMQOptions:HostName");
            Queue = builder.Configuration.GetValue<string>("RabbitMQOptions:Queue");

            Factory = new ConnectionFactory { HostName = RabbitMQApplicationService.HostName };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();

            Channel.QueueDeclare(queue: $"{RabbitMQApplicationService.Queue}return",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            Consumer = new EventingBasicConsumer(Channel);

            Consumer.Received += ReceivedMessage;

            Channel.BasicConsume(queue: $"{RabbitMQApplicationService.Queue}return",
                     autoAck: true,
                     consumer: Consumer);

        }

        public static void ReceivedMessage(object? model, BasicDeliverEventArgs eventArgs) {

            var hub = Provider.GetService<IHubContext<ChatHub>>();

            var body = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var message = JsonConvert.DeserializeObject<RabbitMessageDTO>(json);

            var teste = new IdentityUser() { UserName = message.UserName };

            hub.Clients.All.SendAsync("ReceiveMessage", teste, message.Message);

        }

        public static void SendMessageFromRabbitMQ(string userName, string roomName, string message) {

            var resultDTO = new RabbitMessageDTO() {
                Message = message,
                RoomName = roomName,
                UserName = userName
            };

            var bodyJson = JsonConvert.SerializeObject(resultDTO);
            var bodyResult = Encoding.UTF8.GetBytes(bodyJson);

            Channel.BasicPublish(exchange: string.Empty,
            routingKey: RabbitMQApplicationService.Queue,
            basicProperties: null,
            body: bodyResult);

        }


    }
}
