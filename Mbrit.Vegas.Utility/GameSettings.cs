namespace Mbrit.Vegas.Utility
{
    internal class GameSettings
    {
        internal decimal UnitSize { get; }
        internal decimal SingleStandardDeviation { get; }
        internal decimal CeilingMultipler { get; }
        internal decimal FloorMultipler { get; }
        internal decimal StandardDeviationHigh { get; }
        internal decimal StandardDeviationLow { get; }
        internal decimal StartBank { get; }
        internal int CeilingUnits { get; }
        internal int FloorUnits { get; }
        internal int QuitUnits { get; }

        internal GameSettings(decimal unitSize, decimal expectedVolatility, int numHands, decimal ceilingMultipler, decimal floorMultiplier)
        {
            this.UnitSize = unitSize;

            this.CeilingMultipler = ceilingMultipler;
            this.FloorMultipler = floorMultiplier;

            this.SingleStandardDeviation = ((decimal)Math.Sqrt((double)numHands)) * expectedVolatility * unitSize;

            this.StandardDeviationHigh = this.SingleStandardDeviation * ceilingMultipler;
            this.StandardDeviationLow = 0 - (this.SingleStandardDeviation * floorMultiplier);

            var ceilingUnits = Math.Ceiling(this.StandardDeviationHigh / unitSize);
            if (ceilingUnits == 13)
                ceilingUnits++;
            this.CeilingUnits = (int)ceilingUnits;

            this.FloorUnits = (int)Math.Floor(this.StandardDeviationLow / unitSize);

            this.QuitUnits = (int)((decimal)FloorUnits * .6M);

            var startBank = 0M;
            while (startBank > this.StandardDeviationLow)
                startBank -= 500;
            this.StartBank = 0 - startBank;
        }
    }
}