using ChatRoom.ChatBot.Configurations;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration configuration = builder.Build();

RabbitMQController.Configure(configuration);

while(true) {
    Console.WriteLine("Listening ...");
    Console.ReadLine();
}