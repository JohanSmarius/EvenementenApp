using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Api.Services;
using Shared;

namespace Api
{
    public class EventFunction
    {
        private readonly ILogger<EventFunction> _logger;
        private readonly HappeningService _eventService;
        

        public EventFunction(ILogger<EventFunction> logger, HappeningService eventService)
        {
            _logger = logger;
            this._eventService = eventService;
        }

        [Function("Events")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var events = await _eventService.GetEventsAsync();
            return new OkObjectResult(events);
        }

        [Function("AddEvent")]
        public async Task<IActionResult> AddEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to add an event.");

            var newEvent = await req.ReadFromJsonAsync<Happening>();
            if (newEvent == null)
            {
                return new BadRequestObjectResult("Invalid event data.");
            }

            await _eventService.AddEventAsync(newEvent);
            return new OkObjectResult(newEvent);
        }
    }
}
