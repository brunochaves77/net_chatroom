using ChatRoom.Repository;
using ChatRoom.WebApplication.Configuration;
using ChatRoom.WebApplication.Controllers;
using ChatRoom.WebApplication.Hubs;
using Microsoft.AspNetCore.Identity;

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


builder.ConfigureRepository();
builder.ConfigureService();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("signalr");
app.UseCors("reactApp");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");
app.MapControllers();

app.Run();
