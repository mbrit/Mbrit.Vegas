using Mbrit.Vegas.Web.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Mbrit.Vegas.Web.Api.Controllers
{
    [Route("/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        public IEnumerable<GameDto> GetGames()
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
