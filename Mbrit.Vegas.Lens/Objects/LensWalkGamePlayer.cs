using Mbrit.Vegas.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens
{
    internal class LensWalkGamePlayer : AutomaticWalkGamePlayer
    {
        private Action<int, int, int> ProfitCallback { get; }

        internal LensWalkGamePlayer(Action<int, int, int> profitCallback)
        {
            this.ProfitCallback = profitCallback;
        }

        public override void BankedProfit(int hand, int currencyAmount, int units)
        {
            base.BankedProfit(hand, currencyAmount, units);
            this.ProfitCallback(hand, currencyAmount, units);
        }
    }
}
