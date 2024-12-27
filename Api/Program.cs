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
    var connectionString = string.IsNullOrEmpty(builder.Configuration["ConnectionStrings:CosmosDBConnection"]) ? 
        builder.Configuration["ConnectionStrings_CosmosDBConnection"] : builder.Configuration["ConnectionStrings:CosmosDBConnection"];

    var cosmosClient = new CosmosClient(connectionString);
    return new HappeningService(cosmosClient);
});

builder.Build().Run();
