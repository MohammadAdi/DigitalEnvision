using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DigitalEnvision.Assigment.Features.Location.Queries;

namespace DigitalEnvision.Assigment.Controllers
{
    public class LocationController : Controller
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var result = await _mediator.Send(new GetLocations.Query());
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
