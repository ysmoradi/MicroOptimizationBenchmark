using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace SlowApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            throw new ResourceNotFoundException();
        }
    }

    public interface IHttpStatusCodeAwareException
    {
        HttpStatusCode HttpStatusCode { get; }
    }

    [Serializable]
    public class ResourceNotFoundException : Exception, IHttpStatusCodeAwareException
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message) 
            : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
    }
}
