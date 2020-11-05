using Microsoft.AspNetCore.Mvc;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/ping")]
    public class PingController : ControllerBase
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public string Get()
        {
            return "pong";
        }
    }
}