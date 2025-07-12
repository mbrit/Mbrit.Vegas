using Mbrit.Vegas.Games;
using Mbrit.Vegas.Web.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Mbrit.Vegas.Web.Api
{
    internal static class ControllerBaseExtender
    {
        internal static GameRun GetGameRunAndCheckAccess(this ControllerBase controller, string token)
        {
            var run = GameRun.GetByToken(token);
            return run;
        }
    }
}
