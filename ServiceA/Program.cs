using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Bu, bir Docker ortamýnda servisler arasý baðlantý için doðru yoldur.
// Connection String'leri doðrudan appsettings.json dosyasýndan alýyoruz.
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServer");


builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString, name: "redis-check")
    // MongoClient'ý doðrudan lambda ifadesi içinde oluþturarak hatayý çözdük
    .AddMongoDb(
        sp => new MongoClient(mongoDbConnectionString),
        name: "mongodb-check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new[] { "mongodb" }
    )
    .AddSqlServer(sqlServerConnectionString, name: "sqlserver-check", tags: new[] { "sqlserver" });

var app = builder.Build();
app.UseHealthChecks("/health");
app.Run();
