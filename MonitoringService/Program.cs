using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });

builder.Services.AddHealthChecksUI(setupSettings: setup =>
{
    // URLs of the services to be monitored
    setup.AddHealthCheckEndpoint("ServiceA Health Check", "/healthA");
    setup.AddHealthCheckEndpoint("ServiceB Health Check", "/healthB");
    setup.SetEvaluationTimeInSeconds(5);
    setup.MaximumHistoryEntriesPerEndpoint(50);
    setup.SetApiMaxActiveRequests(1);
}).AddSqlServerStorage("SqlServer");

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

app.MapHealthChecksUI();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("self"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();