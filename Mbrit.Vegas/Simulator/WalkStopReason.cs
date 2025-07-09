using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public enum WalkStopReason
    {
        None = 0,
        HitSpike1 = 1,
        NothingToInvest = 2,
        //EvaluatedDrop = 3,
        HitSpike0p5 = 4,
        HitSpike1p5 = 5,
        HitSpike2 = 6,
        HitSpike3Plus = 7,
        DynamicHandsLimit = 8,
        DynamicInvestmentLimit = 9,
        //NegativeChainScore = 10,
        BadRun = 11,
        GoodRun = 12,
        Bleeding = 13
    }
}
