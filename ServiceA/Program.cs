using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddMongoDb(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")), name: "mongodb-check", failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy, tags: new[] { "mongodb" })
    .AddRedis(builder.Configuration.GetConnectionString("Redis"), name: "redis-check", failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy, tags: new[] { "redis" })
    .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer"), name: "sqlserver-check", failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy, tags: new[] { "sqlserver" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/healthA", new HealthCheckOptions
{
    Predicate = r => r.Tags.Any(),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
