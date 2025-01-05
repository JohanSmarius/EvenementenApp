using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Shared;
using System.Collections.Generic;

namespace Api
{
    public class VolunteerFunction
    {
        private readonly ILogger<VolunteerFunction> _logger;

        public VolunteerFunction(ILogger<VolunteerFunction> logger)
        {
            _logger = logger;
        }

        [Function("VolunteerFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("GetVolunteers")]
        public IActionResult GetVolunteers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "volunteers")] HttpRequest req)
        {
            var volunteers = new List<Volunteer>
            {
                new Volunteer { Id = "1", Name = "Manuela" },
                new Volunteer { Id = "2", Name = "Ineke" },
                new Volunteer { Id = "3", Name = "Saskia" },
                new Volunteer { Id = "4", Name = "Martijn" },
                new Volunteer { Id = "5", Name = "Agnes" },
                new Volunteer { Id = "6", Name = "Johan" }
            };

            return new OkObjectResult(volunteers);
        }
    }
}
