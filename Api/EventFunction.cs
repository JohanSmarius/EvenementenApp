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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Events/{id?}")] HttpRequest req, Guid? id)
        {
            if (id is not null)
            {
                var eventById = await _eventService.GetEventByIdAsync(id.Value);
                return new OkObjectResult(eventById);
            }

            var events = await _eventService.GetEventsAsync();
            return new OkObjectResult(events);
        }

        [Function("AddEvent")]
        public async Task<IActionResult> AddEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            var newEvent = await req.ReadFromJsonAsync<Happening>();
            if (newEvent == null)
            {
                return new BadRequestObjectResult("Invalid event data.");
            }

            await _eventService.AddEventAsync(newEvent);
            return new OkObjectResult(newEvent);
        }

        [Function("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([HttpTrigger(AuthorizationLevel.Anonymous, "put")] HttpRequest req)
        {
            var updatedEvent = await req.ReadFromJsonAsync<Happening>();
            if (updatedEvent == null)
            {
                return new BadRequestObjectResult("Invalid event data.");
            }

            await _eventService.UpdateEventAsync(updatedEvent);
            return new OkObjectResult(updatedEvent);
        }

        [Function("SoftDeleteEvent")]
        public async Task<IActionResult> SoftDeleteEvent([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequest req)
        {
            var eventId = req.Query["id"];
            if (string.IsNullOrEmpty(eventId))
            {
                return new BadRequestObjectResult("Invalid event ID.");
            }

            await _eventService.SoftDeleteEventAsync(Guid.Parse(eventId));
            return new OkObjectResult($"Event with ID {eventId} has been soft deleted.");
        }

        [Function("AddShift")]
        public async Task<IActionResult> AddShift([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "events/{eventId}/shifts")] HttpRequest req, Guid eventId)
        {
            var newShift = await req.ReadFromJsonAsync<Shift>();
            if (newShift == null)
            {
                return new BadRequestObjectResult("Invalid shift data.");
            }

            await _eventService.AddShiftToEventAsync(eventId, newShift);
            return new OkObjectResult(newShift);
        }
    }
}
