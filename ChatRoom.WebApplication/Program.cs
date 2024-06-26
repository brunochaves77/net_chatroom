using ChatRoom.Repository;
using ChatRoom.WebApplication.ApplicationService;
using ChatRoom.WebApplication.Configuration;
using ChatRoom.WebApplication.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.ConfigureJWT();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();
builder.Services
            .AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
                
                options.AddPolicy("reactApp", builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                options.AddPolicy("signalr",
                      builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(hostName => true));
                ;
            });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDataContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<AppDataContext>(o =>
{
    // Make sure the identity database is created
    o.Database.Migrate();
});

builder.ConfigureRabbitMQ();
builder.ConfigureRepository();
builder.ConfigureService();

var app = builder.Build();

RabbitMQApplicationService.Provider = app.Services;

System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("signalr");
app.UseCors("reactApp");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub", options =>
{
    options.TransportMaxBufferSize = 36000;
    options.ApplicationMaxBufferSize = 36000;
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});
app.MapControllers();

app.Run();
