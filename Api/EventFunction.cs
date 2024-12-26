using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Api.Services;

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
    }

    
}
