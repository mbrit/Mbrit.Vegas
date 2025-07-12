using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WalkState : IWalkAdjuster
    {
        // ***************** remember to update clone method *****************
        internal WalkArgs Args { get; }
        internal int InitialUnit { get; }
        internal int CurrentUnit { get; private set; }
        internal int Invested { get; private set; }
        internal int NumInvestments { get; private set; }
        internal bool IsAborted { get; private set; }
        public int Bankroll { get; set; }
        public int Banked { get; set; }
        internal int TotalWagered { get; set; }
        internal int LastInvestment { get; private set; }
        internal int TopUp4BetCount { get; set; }
        internal int HailMarysRemaining { get; set; }

        private List<int> Investables { get; set; }

        private List<Chain> WinChains { get; set; } = new List<Chain>();
        private List<Chain> LossChains { get; set; } = new List<Chain>();

        private Chain WinChain { get; set; }
        private Chain LossChain { get; set; }

        internal WalkStopReason StopReason { get; set; }
        // ***************** remember to update clone method *****************

        internal WalkState(WalkArgs args)
        {
            if (args.HailMaryMode != WalkHailMary.None && args.HailMaryCount <= 0)
                throw new ArgumentException("A Hail Mary count was not provided.");

            this.Args = args;
            this.CurrentUnit = args.Unit;
            this.InitialUnit = args.Unit;

            var investables = new List<int>();
            for (var index = 0; index < args.MaxPutIns; index++)
                investables.Add(this.CurrentUnit);
            this.Investables = investables;

            if (args.HailMaryMode != WalkHailMary.None)
                this.HailMarysRemaining = args.HailMaryCount;
        }

        internal bool HasInvestables => this.Investables.Any();

        internal int InvestablesRemaining => this.Investables.Count();

        internal int TakeProfit => (int)((decimal)this.CurrentUnit * this.Args.TakeProfitMultiplier);

        public int Investable => this.Investables.Sum();

        public int Profit => (this.Bankroll + this.Banked) - this.Invested;

        internal int InvestedUnits => this.CashToCurrentUnits(this.Invested);

        internal int BankrollUnits => this.CashToInitialUnits(this.Bankroll);

        public int PutIn()
        {
            if (!(this.HasInvestables))
                throw new InvalidOperationException("Nothing to invest.");

            var putIn = this.Investables.First();
            this.Investables.RemoveAt(0);

            this.Invested += putIn;
            this.LastInvestment = putIn;
            this.NumInvestments++;

            //invested += toInvest;
            this.Bankroll += putIn;

            return putIn;
        }

        public void PressUnit()
        {
            /*
            var adjustment = 2M;

            this.CurrentUnit = (int)Math.Floor(((decimal)this.CurrentUnit * adjustment));
            while (this.CurrentUnit % 5 != 0)
                this.CurrentUnit++;

            var newInvestables = new List<int>();
            foreach (var value in this.Investables)
                newInvestables.Add(this.CurrentUnit);
            this.Investables = newInvestables;
            */

            throw new NotImplementedException("This operation has not been implemented.");
        }

        public void Abort() => this.IsAborted = true;
        
        internal int CashToInitialUnits(int cash) => cash / this.InitialUnit;

        internal int CashToCurrentUnits(int cash) => cash / this.CurrentUnit;

        internal WalkState Clone()
        {
            var newState = (WalkState)this.MemberwiseClone();
            newState.Investables = new List<int>(this.Investables);
            return newState;
        }

        internal void AddWin()
        {
            if(this.WinChain == null)
            {
                this.WinChain = new Chain();
                this.WinChains.Add(this.WinChain);

                this.LossChain = null;
            }

            this.WinChain.Increment();
        }

        internal void AddLoss()
        {
            if (this.LossChain == null)
            {
                this.LossChain = new Chain();
                this.LossChains.Add(this.LossChain);

                this.WinChain = null;
            }

            this.LossChain.Increment();
        }

        internal decimal WinChainScore => this.WinChains.ScoreChains(true);

        internal decimal LossChainScore => 0 - this.LossChains.ScoreChains(false);

        internal decimal ChainScore => this.WinChainScore + this.LossChainScore;

        internal int MaxLossChain => this.LossChains.Max();

        internal int MaxWinChain => this.WinChains.Max();

        internal decimal EvPer100Currency => (this.Profit / (decimal)this.TotalWagered) * 100M;

        internal WalkPointOutcome GetPointOutcome(int hand) => new WalkPointOutcome(this.Profit, this.TotalWagered, this.EvPer100Currency, hand);
    }
}
