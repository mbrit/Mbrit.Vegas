using BootFX.Common;
using BootFX.Common.Model;
using BootFX.Common.Web.Api;
using Mbrit.Vegas.Games;
using Mbrit.Vegas.Simulator;
using Mbrit.Vegas.Web.Api.Model;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Text.Json;

namespace Mbrit.Vegas.Web.Api.Controllers
{
    internal record WalkPlayerAndRoundsPair(IWalkGamePlayer Player, IWinLoseDrawRoundsBucket Rounds);

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

            Console.WriteLine(JsonSerializer.Serialize(dto));

            return dto;
        }

        [HttpPost("start")]
        public WalkGameStateDto Start()
        {
            var request = this.GetRequest<WalkGameSetupRequest>();

            // create a new game run...
            var game = GameRun.CreateGameRun(request.Mode, request.Unit, request.HailMaryCount, WalkGameDefaults.Investables, WalkGameDefaults.HandsPerRound,
                WalkGameDefaults.HouseEdge);

            // run the game...
            //var player = new InitialManualPlayer();
            //var rounds = new InitialManualGameRounds(game.HouseEdge);
            var pair = this.GetPair(game);
            return this.RunGame(pair, game, true).Dto;
        }

        [HttpGet("{token}")]
        public WalkGameStateDto GetGameState(string token)
        {
            var game = this.GetGameRunAndCheckAccess(token);

            // run the game...
            //var player = new InitialManualPlayer();
            //var rounds = new InitialManualGameRounds(game.HouseEdge);
            var pair = this.GetPair(game);
            var dto = this.RunGame(pair, game, true).Dto;
            return dto;
        }

        private RunGameResult RunGame(WalkPlayerAndRoundsPair pair, GameRun game, bool getDto)
        { 
            var player = pair.Player;
            var rounds = pair.Rounds;

            int lastBankedProfitHand = -1;
            int lastBankedProfitUnits = 0;

            int lastPlayedHand = -1;

            if (player is ManualPlayer)
            {
                ((ManualPlayer)player).BankedProfit += (sender, e) =>
                {
                    lastBankedProfitHand = e.Hand;
                    lastBankedProfitUnits = e.Units;
                };

                ((ManualPlayer)player).HandPlayed += (sender, e) =>
                {
                    lastPlayedHand = e.Item;
                };
            }

            var simulator = new WalkFoo();
            var runs = simulator.DoMagic(rounds, 1, player, (index, setup) =>
            {
                // the move always has to be unrestricted as we need to give the user their leeway to play...
                return WalkGameDefaults.GetSetup(WalkGameMode.Unrestricted, rounds, game.Unit, game.HailMaryCount);
            });

            if (runs.Count() != 1)
                throw new InvalidOperationException("Invalid number of runs.");

            var run = runs.First();
            var results = run.Results;

            if (results.Count() != 1)
                throw new InvalidOperationException("Invalid number of results.");

            var result = results.First();

            if (getDto)
            {
                HandAndUnitsPair lastBankedProfit = null;
                if (lastBankedProfitHand != -1 && lastBankedProfitHand == lastPlayedHand)
                    lastBankedProfit = new HandAndUnitsPair(lastBankedProfitHand, lastBankedProfitUnits);

                var dto = WalkGameStateDto.FromWalkResult(result, player, lastBankedProfit, game);
                return new RunGameResult(result, dto);
            }
            else
                return new RunGameResult(result);
        }

        private WalkPlayerAndRoundsPair GetPair(GameRun game)
        {
            var hands = game.GetHandsOrderedByHand();
            var rounds = new ManualGameRounds(hands, game);
            var player = rounds.GetPlayer();
            return new WalkPlayerAndRoundsPair(player, rounds);
        }

        [HttpGet("{token}/hands/{hand}/actions/{theAction}")]
        public WalkGameStateDto TakeAction(string token, int hand, WalkGameAction theAction) => HandleHand(token, hand, theAction, null, true);

        [HttpGet("{token}/hands/{hand}/outcomes/{outcome}")]
        public WalkGameStateDto SetOutcome(string token, int hand, WinLoseDrawType outcome) => HandleHand(token, hand, null, outcome, false);

        public WalkGameStateDto HandleHand(string token, int hand, WalkGameAction? action, WinLoseDrawType? outcome, bool isAction)
        {
            var game = this.GetGameRunAndCheckAccess(token);

            var pair = this.GetPair(game);

            var player = pair.Player;
            var rounds = pair.Rounds;

            var result = this.RunGame(pair, game, false);

            var state = result.Result.EndState;
            var moves = player.GetNextMoves(hand, rounds[0], state);
            
            var putInMove = moves.Where(v => v is PlayerPutInMove).FirstOrDefault();
            var playMove = moves.Where(v => v is PlayerPlayMove).FirstOrDefault();

            // what are we doing?
            if (isAction)
            {
                if (action == WalkGameAction.Play)
                {
                    if (putInMove != null)
                        game.SetAction(hand, action.Value, ((PlayerPutInMove)putInMove).Units);
                    else if(playMove != null)
                        game.SetAction(hand, action.Value, ((PlayerPlayMove)playMove).Units);
                }
                else
                    throw new NotImplementedException("This operation has not been implemented.");
            }
            else
            {
                game.SetOutcome(hand, outcome.Value);

                // reload the pair and play...
                pair = this.GetPair(game);
            }

            return this.RunGame(pair, game, true).Dto;
        }

        [HttpGet("{token}/abandon")]
        public NullResponse Abandon(string token)
        {
            var game = this.GetGameRunAndCheckAccess(token);
            game.Abandon();
            return new NullResponse();
        }

        /*
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
                            CanPlay = true,
                            Play = 100,
                            PlayUnits = 1,
                            CanHailMary = false,
                            CanWalk = false,
                            Instructions = "Take 1 unit ($100) from your investment pile and play it"
                        },
                        Action = WalkGameAction.None,
                        ProbabilitySpaceAvailableAt = 11
                    }
                }
            };

            return dto;
        }
        */
    }
}
