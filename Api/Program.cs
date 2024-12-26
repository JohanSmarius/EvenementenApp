using Api.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<HappeningService>(_ =>
{
    var cosmosClient = new CosmosClient(
        builder.Configuration["ConnectionStrings:CosmosDBConnection"]);
    return new HappeningService(cosmosClient);
});

builder.Build().Run();
