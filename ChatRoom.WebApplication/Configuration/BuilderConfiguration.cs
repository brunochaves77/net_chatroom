using ChatRoom.Repository;
using ChatRoom.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.WebApplication.Configuration {
    public static class BuilderConfiguration {

        public static void ConfigureRepository(this WebApplicationBuilder builder) {

            var cnnString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (cnnString == null)
                throw new Exception("ConnectionString 'DefaultConnection' not informed on appSettings.");

            builder.Services.AddDbContext<AppDataContext>(options => options.UseSqlServer(cnnString, b => b.MigrationsAssembly("ChatRoom.WebApplication")));

            builder.Services.AddScoped<RoomRepository>();
            builder.Services.AddScoped<ChatMessageRepository>();

        }

    }
}
