using Microsoft.Azure.Cosmos;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services
{
    public class HappeningService
    {
        private readonly CosmosClient cosmosClient; 
        private readonly Container _container;

        public HappeningService(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
            _container = cosmosClient.GetContainer("events", "events");
        }

        public async Task<List<Happening>> GetEventsAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Happening>(query);
            var results = new List<Happening>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task AddEventAsync(Happening newEvent)
        {
            newEvent.id = Guid.NewGuid();
            await _container.CreateItemAsync(newEvent, new PartitionKey(newEvent.id.ToString()));
        }

        public async Task UpdateEventAsync(Happening updatedEvent)
        {
            await _container.UpsertItemAsync(updatedEvent, new PartitionKey(updatedEvent.id.ToString()));
        }

        public async Task SoftDeleteEventAsync(Guid eventId)
        {
            var eventToDelete = await _container.ReadItemAsync<Happening>(eventId.ToString(), new PartitionKey(eventId.ToString()));
            if (eventToDelete != null)
            {
                eventToDelete.Resource.IsDeleted = true;
                await _container.UpsertItemAsync(eventToDelete.Resource, new PartitionKey(eventId.ToString()));
            }
        }
    }
}
