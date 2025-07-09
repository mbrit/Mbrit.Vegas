namespace Mbrit.Vegas.Simulator
{
    public interface IWalkGameSetup
    {
        int UnitSize { get; }
        decimal TakeProfitMultiplier { get; }
        int MaxPutIns { get; }
        int HandsPerRound { get; }
        IWinLoseDrawRoundsBucket Rounds { get; }
        decimal HouseEdge { get; }

        WalkArgs GetArgs();
    }
}