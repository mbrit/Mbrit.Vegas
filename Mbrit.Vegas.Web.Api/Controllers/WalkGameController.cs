using BootFX.Common.Web.Api;
using Mbrit.Vegas.Simulator;
using Mbrit.Vegas.Web.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Mbrit.Vegas.Web.Api.Controllers
{
    [Route("/walk-game")]
    [ApiController]
    public class WalkGameController : ControllerBase
    {
        [HttpPost("projection")]
        public WalkGameProjectionDto RunProjection()
        {
            var request = this.GetRequest<WalkGameSetupRequest>();

            var rand = new Random();
            var rounds = WinLoseDrawRoundsBucket.GetWinLoseBucket(25000, WalkGameDefaults.HandsPerRound, WalkGameDefaults.HouseEdge, rand);

            var order = new List<WalkGameMode>()
            {
                WalkGameMode.ReachSpike0p5,
                WalkGameMode.StretchToSpike1,
                WalkGameMode.ReachSpike1,
                WalkGameMode.Unrestricted,
            };

            var simulator = new WalkFoo();
            var runs = simulator.DoMagic(rounds, 4, (index, setup) =>
            {
                return WalkGameDefaults.GetSetup(order[index], rounds, request.Unit, request.HailMaryCount);
            });

            // results...
            var dto = new WalkGameProjectionDto();

            var asList = runs.ToList();
            for(var index = 0; index < order.Count; index++)
            {
                var outcomes = asList[index].GetOutcomes();

                var item = new WalkGameProjectionItemDto()
                {
                    Mode = order[index],
                    Outcomes = outcomes.ToDto()
                };
                dto.Outcomes.Add(item);
            }

            return dto;
        }
    }
}
