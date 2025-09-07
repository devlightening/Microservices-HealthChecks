using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServer");


builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString, name: "redis-check")
    .AddMongoDb(
        mongoDbConnectionString,
        name: "mongodb-check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new[] { "mongodb" }
    )
    .AddSqlServer(sqlServerConnectionString, name: "sqlserver-check", tags: new[] { "sqlserver" });

var app = builder.Build();


app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
