using Mbrit.Vegas.Simulator;
using Mbrit.Vegas.Web.Api.Model;

namespace Mbrit.Vegas.Web.Api.Controllers
{
    internal class RunGameResult
    {
        internal WalkResult Result { get; }
        internal WalkGameStateDto Dto { get; }

        internal RunGameResult(WalkResult result, WalkGameStateDto dto = null)
        {
            this.Result = result;
            this.Dto = dto;
        }
    }
}