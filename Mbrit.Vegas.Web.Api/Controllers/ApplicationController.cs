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

        [HttpGet("strings/en-us/version")]
        public ScalarResponse<int> GetStringVersion() => new ScalarResponse<int>(1);

        [HttpGet("strings/en-us/{version}")]
        public Dictionary<string, string> GetStrings(int version)
        {
            var values = new Dictionary<string, string>();
            values["splash/main-title"] = "The Vegas Walk Methodx";
            values["splash/sub-title"] = "The only method that teachs a 30% chance to DOUBLE YOUR MONEY in Vegasx";
            return values;
        }
    }
}
