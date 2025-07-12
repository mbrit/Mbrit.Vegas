using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class BankedProfitEventArgs : EventArgs
    {
        public int Hand { get; }
        public int CurrencyAmount { get; }
        public int Units { get; }

        internal BankedProfitEventArgs(int hand, int currencyAmount, int units)
        {
            this.Hand = hand;
            this.CurrencyAmount = currencyAmount;
            this.Units = units;
        }
    }
}
