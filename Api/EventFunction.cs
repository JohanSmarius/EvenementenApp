using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Api
{
    public class EventFunction
    {
        private readonly ILogger<EventFunction> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public EventFunction(ILogger<EventFunction> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer("databaseId", "containerId");
        }

        [Function("Events")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var events = await GetEventsAsync();
            return new OkObjectResult(events);
        }

        private async Task<List<Event>> GetEventsAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Event>(query);
            var results = new List<Event>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }

    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public List<Event> ChildEvents { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<Volunteer> AssignedVolunteers { get; set; }
    }

    public class Shift
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public List<Volunteer> AssignedVolunteers { get; set; }
    }

    public class Volunteer
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
