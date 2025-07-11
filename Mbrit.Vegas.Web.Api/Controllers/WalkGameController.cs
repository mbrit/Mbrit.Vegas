using BootFX.Common;
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
            var rounds = WinLoseDrawRoundsBucket.GetPrebakedWinLoseBucket(25000, WalkGameDefaults.HandsPerRound, WalkGameDefaults.HouseEdge, rand);

            var order = new List<WalkGameMode>()
            {
                WalkGameMode.ReachSpike0p5,
                WalkGameMode.StretchToSpike1,
                WalkGameMode.ReachSpike1,
                WalkGameMode.Unrestricted,
            };

            var player = new AutomaticWalkGamePlayer();

            var simulator = new WalkFoo();
            var runs = simulator.DoMagic(rounds, 4, player, (index, setup) =>
            {
                // lookup the variant from the list above...
                return WalkGameDefaults.GetSetup(order[index], rounds, request.Unit, request.HailMaryCount);
            });

            // results...
            var dto = new WalkGameProjectionDto();

            var asList = runs.ToList();
            for (var index = 0; index < order.Count; index++)
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

        [HttpPost("start")]
        public WalkGameStateDto StartGame()
        {
            var request = this.GetRequest<WalkGameSetupRequest>();

            var piles = new WalkGamePilesDto()
            {
                Investable = request.Unit * 12,
                InvestableUnits = 12,
                InPlay = 0,
                InPlayUnits = 0,
                Banked = 0,
                BankedUnits = 0,
                CashInHand = request.Unit * 12,
                CashInHandUnits = 12,
                Profit = 0,
                ProfitUnits = 0
            };

            var dto = new WalkGameStateDto()
            {
                Token = "12345",
                Piles = piles,
                Hands = new List<WalkGameHandDto>()
                {
                    new WalkGameHandDto()
                    {
                        Index = 0,
                        IsDraft = true,
                        NeedsAnswer = false,
                        PilesBefore = piles,
                        Casino = new CasinoDto()
                        {
                            Name = "Flamingo",
                            Location = new LocationDto()
                            {
                                Name = "Las Vegas Strip"
                            }
                        },
                        Game = new GameItemDto()
                        {
                            Name = "Baccarat Banker Bet",
                            HouseEdge = 0.016M,
                        },
                        Actions = new WalkGameActionsDto()
                        {
                            CanPutInAndPlay = true,
                            CanPutInUnits = 1,
                            CanPlay = false,
                            CanHailMary = false,
                            CanWalk = false
                        },
                        Action = WalkGameAction.None,
                        ProbabilitySpaceAvailableAt = 11
                    }
                }
            };

            return dto;
        }
    }
}
