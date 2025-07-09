using BootFX.Common.Model;
using Mbrit.Vegas.Web.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Mbrit.Vegas.Web.Api.Controllers
{
    [ApiController]
    [Route("/application")]
    public class ApplicationController : ControllerBase
    {
        [HttpGet("ping")]
        public PingResponse Ping() => PingResponse.AssertDefaultDatabaseAndGetPingResponse();
    }
}
