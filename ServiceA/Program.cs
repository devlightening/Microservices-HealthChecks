using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("Redis"), name: "redis-check")
    .AddMongoDb(
        mongoDbConnectionString: builder.Configuration.GetConnectionString("MongoDb"),
        name: "mongodb-check",
        databaseName: "your_database_name");













var app = builder.Build();
app.UseHealthChecks("/health");


app.Run();

