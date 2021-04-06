namespace Example.AspCore.Controllers
{
    using System;

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
    }
}
