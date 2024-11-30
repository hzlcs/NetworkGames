using GameService.Entity.DBEntity;
using GameService.Entity.Test.Games;
using GameService.Entity.Test.Matchs;
using GameService.Entity.Test.UserManage;
using GameService.Hubs;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using GameService.Interfaces.Test.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureService(builder.Services);
string? connectionString = builder.Configuration.GetConnectionString("test");
builder.Services.AddSqlServer<MyDbContext>(connectionString);
// string? connectionString = builder.Configuration.GetConnectionString("sqlite");
// builder.Services.AddSqlite<MyDbContext>(connectionString);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapHub<GameHub>("/GameHub", v => v.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets);
app.MapHub<MatchHub>("/MatchHub", v => v.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets);

app.Run("http://*:5001");
return;

static void ConfigureService(IServiceCollection services)
{
    services.AddControllers();
    services.AddLogging(v => v.AddSimpleConsole());
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSignalR();
    services.AddScoped<IUserManager, UserManager>();
    services.AddSingleton<IGameRepository, GameRepository>();
    services.AddSingleton<IMatchGroupManager, MatchGroupManager>();
    services.AddSingleton<IMatchManager, MatchManager>();

}
