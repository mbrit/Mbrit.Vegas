using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WalkArgs
    {
        internal int BaseUnit { get; set; }
        internal int MainUnit { get; set; }
        internal int SideUnit { get; set; }
        internal decimal SideSplit { get; set; }
        internal decimal TakeProfitMultiplier { get; set; }
        internal decimal MinorBustMultiplier { get; set; }
        internal int MaxHands { get; set; }
        internal int MaxPutIns { get; set; }

        internal int MinorBust { get; }
        internal int MinorWin { get; }
        internal int Spike { get; }
        internal int InitialMaxInvestment { get; }

        internal const int DefaultNumHands = 25;

        internal WalkArgs(int baseUnit, decimal takeProfitMultiplier, int maxPutIns, int maxHands = DefaultNumHands)
        {
            this.BaseUnit = baseUnit;
            this.SideSplit = 0;
            this.TakeProfitMultiplier = takeProfitMultiplier;
            this.MaxPutIns = maxPutIns;
            this.MaxHands = maxHands;

            var mainUnit = (int)Math.Round(this.BaseUnit * this.SideSplit);
            while (mainUnit % 5 != 0)
                mainUnit--;

            this.MainUnit = baseUnit;
            this.SideUnit = this.BaseUnit - baseUnit;

            this.MinorBustMultiplier = 3M;
            this.MinorBust = 0 - (int)((decimal)this.Unit * this.MinorBustMultiplier);

            this.InitialMaxInvestment = this.Unit * maxPutIns;
            this.Spike = this.InitialMaxInvestment;                 // happens to be "making our money back"...
            this.MinorWin = (int)((decimal)this.Spike * 0.5M);      // happens to be "making half out money back"...
        }

        internal int Unit => this.MainUnit + this.SideUnit;
    }
}
