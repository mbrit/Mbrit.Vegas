using Mbrit.Vegas.Simulator;

namespace Mbrit.Vegas
{
    public interface IPermutationSelector
    {
        WalkGameMode Mode { get; }
        int Hands { get; }
        int Investables { get; }
        int UnitSize { get; }
        decimal HouseEdge { get; }
        WalkHailMary HailMaryMode { get; }
    }
}