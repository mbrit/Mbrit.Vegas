using Mbrit.Vegas.Simulator;

namespace Mbrit.Vegas.Web.Api.Model
{
    internal static class ModelExtender
    {
        internal static WalkOutcomesBucketDto ToDto(this WalkOutcomesBucket item, DtoLoader loader = null)
        {
            loader = loader ?? new DtoLoader();

            return new WalkOutcomesBucketDto()
            {
                Name = item.Name,
                EvensPercentage = item.EvensPercentage,
                MajorBustPercentage = item.MajorBustPercentage,
                MinorBustPercentage = item.MinorBustPercentage,
                Spike0p5Percentage = item.Spike0p5Percentage,
                Spike1Percentage = item.Spike1Percentage,
                Spike1PlusPercentage = item.Spike1PlusPercentage,
                AverageProfitWhenWon = item.AveragePositiveBankroll,
                AverageCoinIn = item.AverageWagered
            };
        }

        internal static IEnumerable<WalkOutcomesBucketDto> ToDtos(this IEnumerable<WalkOutcomesBucket> items, DtoLoader loader = null)
        {
            loader = loader ?? new DtoLoader();
            return items.Select(v => v.ToDto(loader));
        }

    }
}
