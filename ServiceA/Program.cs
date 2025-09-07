using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Bu, bir Docker ortam�nda servisler aras� ba�lant� i�in do�ru yoldur.
// Connection String'leri do�rudan appsettings.json dosyas�ndan al�yoruz.
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServer");


builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString, name: "redis-check")
    // MongoClient'� do�rudan lambda ifadesi i�inde olu�turarak hatay� ��zd�k
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
