using Mbrit.Vegas.Simulator;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGamePilesDto
    {
        [JsonPropertyName("investable")]
        public int Investable { get; set; }

        [JsonPropertyName("investableUnits")]
        public int InvestableUnits { get; set; }

        [JsonPropertyName("inPlay")]
        public int InPlay { get; set; }

        [JsonPropertyName("inPlayUnits")]
        public int InPlayUnits { get; set; }

        [JsonPropertyName("banked")]
        public int Banked { get; set; }

        [JsonPropertyName("bankedUnits")]
        public int BankedUnits { get; set; }

        [JsonPropertyName("cashInHand")]
        public int CashInHand { get; set; }

        [JsonPropertyName("cashInHandUnits")]
        public int CashInHandUnits { get; set; }

        [JsonPropertyName("profit")]
        public int Profit { get; set; }

        [JsonPropertyName("profitUnits")]
        public int ProfitUnits { get; set; }

        internal static WalkGamePilesDto FromWalkState(WalkState state, GameRun game)
        {
            var dto = new WalkGamePilesDto()
            {
                Investable = state.Investable,
                InPlay = state.Bankroll,
                Banked = state.Banked,
                Profit = state.Profit,
            };

            dto.CashInHand = dto.Investable + dto.InPlay + dto.Banked;

            dto.TouchUnits(game.Unit);

            return dto;
        }

        private void TouchUnits(int unit)
        {
            this.InvestableUnits = this.Investable / unit;
            this.InPlayUnits = this.InPlay / unit;
            this.BankedUnits = this.Banked / unit;
            this.ProfitUnits = this.Profit / unit;
            this.CashInHandUnits = this.CashInHand / unit;
        }
    }
}
