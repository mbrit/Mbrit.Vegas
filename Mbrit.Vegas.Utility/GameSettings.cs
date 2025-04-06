namespace Mbrit.Vegas.Utility
{
    internal class GameSettings
    {
        internal decimal UnitSize { get; }
        internal decimal SingleStandardDeviation { get; }
        internal decimal CeilingMultipler { get; }
        internal decimal FloorMultipler { get; }

        internal decimal StopLossSd { get; }
        internal decimal TakeProfitSd { get; }

        internal decimal StartBank { get; }
        internal bool HasBank { get; }

        internal int StopLossUnits { get; }
        internal int TakeProfitsUnits { get; }
        internal int QuitUnits { get; }

        internal bool HasStopLoss { get; }
        internal bool HasTakeProfits { get; }
        internal bool HasQuitUnits { get; }

        internal GameSettings(decimal unitSize, decimal expectedVolatility, int numHands, decimal stopLossMultiplier, decimal takeProfitsMultipler, bool hasQuitUnits)
        {
            this.UnitSize = unitSize;

            this.CeilingMultipler = takeProfitsMultipler;
            this.FloorMultipler = stopLossMultiplier;

            this.SingleStandardDeviation = ((decimal)Math.Sqrt((double)numHands)) * expectedVolatility * unitSize;

            if (stopLossMultiplier != 0)
            {
                this.StopLossSd = 0 - (this.SingleStandardDeviation * stopLossMultiplier);
                this.StopLossUnits = (int)Math.Floor(this.StopLossSd / unitSize);
                this.HasStopLoss = true;

                if (hasQuitUnits)
                {
                    this.QuitUnits = (int)((decimal)StopLossUnits * .6M);
                    this.HasQuitUnits = true;
                }

                var startBank = 0M;
                while (startBank > this.StopLossSd)
                    startBank -= 500;
                this.StartBank = 0 - startBank;
                this.HasBank = true;
            }

            if (takeProfitsMultipler != 0)
            {
                this.TakeProfitSd = this.SingleStandardDeviation * takeProfitsMultipler;
                this.HasTakeProfits = true;

                var takeProfitsUnits = Math.Ceiling(this.TakeProfitSd / unitSize);
                if (takeProfitsUnits == 13)
                    takeProfitsUnits++;
                this.TakeProfitsUnits = (int)takeProfitsUnits;
            }
        }
    }
}