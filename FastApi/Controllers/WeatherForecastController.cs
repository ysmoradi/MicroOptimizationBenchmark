using Microsoft.AspNetCore.Mvc;

namespace FastApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return NotFound();
        }
    }
}
