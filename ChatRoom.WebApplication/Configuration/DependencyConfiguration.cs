using ChatRoom.Application.Services;

namespace ChatRoom.WebApplication.Configuration {
    public static class DependencyConfiguration {

        public static void ConfigureService(this WebApplicationBuilder builder) {

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<RoomService>();
            builder.Services.AddScoped<ChatMessageService>();

        }


    }
}
