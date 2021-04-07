namespace Example.AspCore.Controllers
{
    using System;

    using Example.AspCore.Commands.Test;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult<string> Index()
        {
            return $"Example Api {DateTime.UtcNow:u}";
        }

        [HttpPost("comms")]
        [ProducesResponseType(typeof(PayloadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Commands([FromBody] Payload payload)
        {
            return Ok();
        }
    }

    public class PayloadResponse
    {
        public Guid CorrelationId { get; set; }
        public bool Executed { get; set; }
        public string CommandName { get; set; }
    }

    public class Payload
    {
        public string CommandName { get; set; }

        public TestCommand CommandBody { get; set; }
    }
}
