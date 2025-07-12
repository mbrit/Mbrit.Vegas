using BootFX.Common.Management;
using Mbrit.Vegas.Games;
using Mbrit.Vegas.Simulator;
using Mbrit.Vegas.Web.Api.Controllers;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameStateDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("piles")]
        public WalkGamePilesDto Piles { get; set; }

        [JsonPropertyName("hands")]
        public List<WalkGameHandDto> Hands { get; set; } = new List<WalkGameHandDto>();

        [JsonPropertyName("probabilitySpace")]
        public WalkGameProbabilitySpaceDto ProbabilitySpace { get; set; }

        [JsonPropertyName("hasProbabilitySpace")]
        public bool HasProbabilitySpace => this.ProbabilitySpace != null;

        [JsonPropertyName("probabilitySpaceAvailableAt")]
        public int ProbabilitySpaceAvailableAt { get; set; }

        [JsonPropertyName("spike0p5")]
        public int Spike0p5 { get; set; }

        [JsonPropertyName("spike0p5Units")]
        public int Spike0p5Units { get; set; }

        [JsonPropertyName("spike1")]
        public int Spike1 { get; set; }

        [JsonPropertyName("spike1Units")]
        public int Spike1Units { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        internal static WalkGameStateDto FromWalkResult(WalkResult result, IWalkGamePlayer player, HandAndUnitsPair lastBankedProfit, GameRun game)
        {
            var state = result.EndState;

            var dto = new WalkGameStateDto()
            {
                Token = game.Token,
                Name = game.Name,
                ProbabilitySpaceAvailableAt = 5,
                Spike0p5 = game.Spike0p5Win,
                Spike1 = game.Spike1Win,
                Currency = game.Currency
            };

            dto.TouchUnits(game);

            // load the hands...
            var hands = game.GetHandsOrderedByHand().ToList();
            for(var handIndex = 0; handIndex < hands.Count; handIndex++)
            {
                var hand = hands[handIndex];

                var handDto = new WalkGameHandDto()
                {
                    Index = handIndex,
                    IsDraft = false,
                    Casino = GetDefaultCasino(),
                    Game = GetDefaultGame(),
                    Action = WalkGameAction.Play,
                    Outcome = hand.Outcome,
                };

                dto.Hands.Add(handDto);
            }

            // what now?
            WalkGameActionsDto actions = null;

            // what moves do we have?
            var rounds = new ManualGameRounds(hands, game);
            var moves = player.GetNextMoves(hands.Count, rounds[0], state);

            PlayerPutInMove putInMove = null;
            PlayerPlayMove playMove = null;
            PlayerWalkMove walkMove = null;

            if (moves.Any())
            {
                foreach (var move in moves)
                {
                    if (move is PlayerPutInMove)
                        putInMove = (PlayerPutInMove)move;
                    else if (move is PlayerPlayMove)
                        playMove = (PlayerPlayMove)move;
                    else if (move is PlayerWalkMove)
                        walkMove = (PlayerWalkMove)move;
                }
            }

            if (!(game.IsWaitingOnDecision))
            {
                actions = new WalkGameActionsDto();

                var builder = new StringBuilder();

                if (putInMove != null)
                {
                    actions.CanPlay = true;
                    actions.PlayUnits = ((PlayerPutInMove)putInMove).Units;
                    actions.TouchCurrency(game.Unit);

                    builder.Append("Take ");
                    builder.Append(game.FormatUnits(actions.PlayUnits));
                    builder.Append(" (");
                    builder.Append(game.UnitsToFormattedCurrency(actions.PlayUnits));
                    builder.Append(") from your investment stack and play it.");
                }

                if(playMove != null)
                {
                    actions.CanPlay = true;
                    actions.PlayUnits = ((PlayerPlayMove)playMove).Units;
                    actions.TouchCurrency(game.Unit);

                    if (lastBankedProfit != null)
                    {
                        builder.Append("Take ");
                        builder.Append(game.FormatUnits(lastBankedProfit.Units));
                        builder.Append(" (");
                        builder.Append(game.UnitsToFormattedCurrency(actions.PlayUnits));
                        builder.Append(") from winnings and bank it, and continue to press with ");
                        builder.Append(game.FormatUnits(actions.PlayUnits));
                        builder.Append(" (");
                        builder.Append(game.UnitsToFormattedCurrency(actions.PlayUnits));
                        builder.Append(") total bet.");
                    }
                    else
                    {
                        builder.Append("Add last win to previous bet to give ");
                        builder.Append(game.FormatUnits(actions.PlayUnits));
                        builder.Append(" (");
                        builder.Append(game.UnitsToFormattedCurrency(actions.PlayUnits));
                        builder.Append(") total bet.");
                    }
                }

                actions.Instructions = builder.ToString();

                if (walkMove != null)
                    actions.CanWalk = true;
            }

            // are we waiting on a decision? if so, the amount in play have to change the piles...
            if (game.IsWaitingOnDecision)
            {
                var toMove = 0;

                if (game.Action == WalkGameAction.Play && putInMove != null)
                    toMove = game.ActionUnits;

                if(toMove > 0)
                {
                    for (var index = 0; index < toMove; index++)
                        state.PutIn();
                }
            }

            // add the hand...
            var draftHand = new WalkGameHandDto()
            {
                Index = hands.Count,
                IsDraft = true,
                NeedsAnswer = game.IsWaitingOnDecision,
                Casino = GetDefaultCasino(),
                Game = GetDefaultGame(),
                //HasSeenSpike0p5 = state.HasSeenSpike0p5,
                //IsAtOrOverSpike0p5 = state.IsAtOrOverSpike0p5,
                //HasSeenSpike1 = state.HasSeenSpike1,
                //IsAtOrOverSpike1 = state.IsAtOrOverSpike1,
                //PilesBefore = WalkGamePilesDto.FromWalkState(state, game),
                //ProbabilitySpaceAvailableAt = 5,
                Actions = actions,
                Outcome = WinLoseDrawType.Win       // supersition...
            };

            dto.Hands.Add(draftHand);

            dto.Piles = WalkGamePilesDto.FromWalkState(state, game);

            // return...
            return dto;
        }

        private void TouchUnits(GameRun game)
        {
            this.Spike0p5Units = game.CurrencyToUnits(this.Spike0p5);
            this.Spike1Units = game.CurrencyToUnits(this.Spike1);
        }

        private static CasinoDto GetDefaultCasino()
        {
            return new CasinoDto()
            {
                Name = "Flamingo",
                Location = new LocationDto()
                {
                    Name = "Las Vegas Strip"
                }
            };
        }

        private static GameItemDto GetDefaultGame()
        {
            return new GameItemDto()
            {
                Name = "Baccarat Banker Bet",
                HouseEdge = 0.016M,
            };
        }
    }
}
