using ChatRoom.WebApplication.Configuration;
using ChatRoom.WebApplication.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services
            .AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );

                options.AddPolicy("signalr",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()

                    .AllowCredentials()
                    .SetIsOriginAllowed(hostName => true));
            });


//builder.ConfigureRepository();
builder.ConfigureService();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("signalr");
app.UseRouting();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");

app.Run();
