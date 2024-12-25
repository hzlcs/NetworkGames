using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GameService.Entity.DBEntity;
using GameService.Entity.Test.Games;
using GameService.Entity.Test.Matchs;
using GameService.Entity.Test.UserManage;
using GameService.Hubs;
using GameService.Interfaces.Test.Games;
using GameService.Interfaces.Test.Matchs;
using GameService.Interfaces.Test.Users;
using GameService.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigureService(builder.Services, builder.Configuration);
if (Environment.UserName == "songfeifan")
{
    string? connectionString = builder.Configuration.GetConnectionString("test");
    builder.Services.AddSqlServer<MyDbContext>(connectionString);
}
else
{
    string? connectionString = builder.Configuration.GetConnectionString("sqlite");
    builder.Services.AddSqlite<MyDbContext>(connectionString);
}

var app = builder.Build();
//跨域
app.UseCors(b =>
{
    b.SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .WithMethods("GET", "POST")
        .AllowCredentials();
});
app.Services.CreateScope().ServiceProvider.GetRequiredService<MyDbContext>().Database.EnsureCreated();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/ChatHub");
app.MapHub<GameHub>("/GameHub", v => v.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets);
app.MapHub<MatchHub>("/MatchHub", v => v.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets);

app.Run("http://*:5000");
return;

static void ConfigureService(IServiceCollection services, ConfigurationManager configuration)
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
    services.AddMemoryCache();
    services.AddRedis("localhost:6379");
    services.AddStackExchangeRedisCache(v =>
    {
        v.Configuration = "localhost:6379";
        v.InstanceName = "GameService";
    });
    //jwt
    services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true, //是否验证Issuer
                ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
                ValidateAudience = true, //是否验证Audience
                ValidAudience = configuration["Jwt:Audience"], //订阅人Audience
                ValidateIssuerSigningKey = true, //是否验证SecurityKey
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)), //SecurityKey
                ValidateLifetime = true, //是否验证失效时间
                ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                RequireExpirationTime = true,
                
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/ChatHub"))
                    {
                        // Read the token out of the query string
                        context.Token = context.Request.Headers.Authorization.ToString()[("Bearer ".Length)..];
                        context.Principal = new JwtSecurityTokenHandler().ValidateToken(context.Token, options.TokenValidationParameters, out var securityToken);
                    }
                    return Task.CompletedTask;
                }
            };
        });
    services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
    services.AddSingleton(new JwtHelper(configuration));
}

public class NameUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.Identity?.Name;
    }
}


