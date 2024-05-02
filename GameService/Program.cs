using GameService.Entity.DBEntity;
using GameService.Entity.Test.Games;
using GameService.Entity.Test.Matchs;
using GameService.Hubs;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureService(builder.Services);
string? connectionString = builder.Configuration.GetConnectionString("sqlserver");
builder.Services.AddSqlServer<MyDbContext>(connectionString);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapHub<GameHub>("/GameHub");
app.MapHub<MatchHub>("/MatchHub");
app.Run();

static void ConfigureService(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSignalR();
    services.AddSingleton<IGameRepository, GameRepository>();
    services.AddSingleton<IMatchGroupManager, MatchGroupManager>();
    services.AddSingleton<IMatchManager, MatchManager>();
    
}
