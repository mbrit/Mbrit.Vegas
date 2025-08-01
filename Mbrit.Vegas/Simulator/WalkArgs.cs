﻿using BootFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WalkArgs
    {
        internal string Name { get; set; }

        internal int BaseUnit { get; set; }
        internal int MainUnit { get; set; }
        internal int SideUnit { get; set; }
        internal int InitialUnit { get; set; }
        internal decimal SideSplit { get; set; }
        internal decimal TakeProfitMultiplier { get; set; }
        //internal decimal InitialTakeProfitMultiplier { get; set; }
        internal decimal MinorBustMultiplier { get; set; }
        internal decimal InitialMinorBustMultiplier { get; set; }
        //internal decimal MinorWinMultiplier { get; set; }
        //internal decimal InitialMinorWinMultiplier { get; set; }
        internal int MaxHands { get; set; }
        internal int MaxPutIns { get; set; }

        internal int MinorBust { get; }
        //internal int MinorWin { get; }
        public int Spike0p5Win { get; private set; }      // set this with caution...
        public int Spike1Win { get; }
        internal int Spike1p5Win { get; }
        internal int Spike2Win { get; } 
        internal int Spike3Win { get; }
        internal int InitialMaxInvestment { get; }
        internal int InitialTakeProfit { get; }

        //internal bool UseEvaluations { get; set; } = false;

        //internal bool TopUp4Bet { get; set; } = false;
        //internal int TopUp4BetFrequency { get; set; } = 1;
        //internal int MaxTopUp4Bets { get; set; }

        public WalkGameMode StopAtWinMode { get; set; } = WalkGameMode.ReachSpike1;

        internal int LimitedStretchHands { get; set; } = -1;
        internal int LimitedStretchInvestments { get; set; } = -1;
        internal bool DoEvaluation { get; set; }
        internal decimal StretchDeclineThreshhold { get; set; } = 0.55M;

        //internal int StopAtMinusUnits { get; set; } = -1;

        //internal decimal? AbandonAtOrUnderChainScore { get; set; }
        //internal int BadRunLimit { get; set; } = -1;
        //internal int GoodRunLimit { get; set; } = -1;

        //internal bool AddInvestablesToFinal { get; set; }
        //internal bool StopAtSpike { get; set; }

        public WalkHailMary HailMaryMode { get; set; } = WalkHailMary.None;           // ideally, this should be single and 1 count...
        public int HailMaryCount { get; set; }

        internal WalkArgs(IWalkGameSetup setup, string name = null)
        {
            this.BaseUnit = setup.UnitSize;
            this.SideSplit = 0;
            this.TakeProfitMultiplier = setup.TakeProfitMultiplier;
            this.MaxPutIns = setup.MaxPutIns;
            this.MaxHands = setup.HandsPerRound;

            var mainUnit = (int)Math.Round(this.BaseUnit * this.SideSplit);
            while (mainUnit % 5 != 0)
                mainUnit--;

            this.MainUnit = setup.UnitSize;
            this.SideUnit = 0; // this.MainUnit - baseUnit;
            this.InitialUnit = this.Unit;

            //this.MinorBustMultiplier = this.TakeProfitMultiplier; // 4M;
            this.MinorBustMultiplier = this.MaxPutIns / 2; // 6M;
            this.InitialMinorBustMultiplier = this.MinorBustMultiplier;
            this.MinorBust = 0 - (int)((decimal)this.Unit * this.MinorBustMultiplier);

            this.InitialMaxInvestment = this.Unit * setup.MaxPutIns;
            this.InitialTakeProfit = (int)(this.Unit * this.TakeProfitMultiplier);

            this.Spike0p5Win = (int)((decimal)this.InitialMaxInvestment * 0.5M);                 // happens to be "making our money back"...
            this.Spike1Win = (int)((decimal)this.InitialMaxInvestment * 1M);                 // happens to be "making our money back"...
            this.Spike1p5Win = (int)((decimal)this.InitialMaxInvestment * 1.5M);
            this.Spike2Win = (int)((decimal)this.InitialMaxInvestment * 2M);   
            this.Spike3Win = (int)((decimal)this.InitialMaxInvestment * 3M);

            /*
            this.MinorWinMultiplier = (int)((decimal)setup.MaxPutIns * 0.5M);
            this.InitialMinorWinMultiplier = this.MinorWinMultiplier;
            this.MinorWin = (int)((decimal)this.Unit * this.MinorWinMultiplier);      // happens to be "making half out money back"...
            */
        }

        internal int Unit => this.MainUnit + this.SideUnit;

        internal bool HasName => !(string.IsNullOrEmpty(this.Name));

        /*
        internal int StopAtWin
        {
            get
            {
                if (this.StopAtWinType == WalkStopType.ReachedSpike0p5)
                    return this.Spike0p5Win;
                else if (this.StopAtWinType == WalkStopType.ReachedSpike1)
                    return this.Spike1Win;
                else
                    throw new NotSupportedException($"Cannot handle '{this.StopAtWinType}'.");
            }
        }
        */

        internal bool DoStopAtWin => this.StopAtWinMode != WalkGameMode.Unrestricted;

        internal void SetSpike0p5Win(int value) => this.Spike0p5Win = value;

        public void SetStretchToSpike1()
        {
            this.StopAtWinMode = WalkGameMode.StretchToSpike1;

            // this one seems stable...
            this.LimitedStretchHands = 6;
            this.LimitedStretchInvestments = -1;

            //this.DoEvaluation = true;
        }

        internal void SetHailMary(int hailMaryCount)
        {
            if (hailMaryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(hailMaryCount));

            if(hailMaryCount == 1 || hailMaryCount == 2)
            {
                this.HailMaryMode = WalkHailMary.Single;        // seems to be the one that works...
                this.HailMaryCount = hailMaryCount;
            }
            else if(hailMaryCount == 0)
            {
                this.HailMaryMode = WalkHailMary.None;
                this.HailMaryCount = 0;
            }
            else
                throw new NotSupportedException($"Cannot handle '{hailMaryCount}'.");
        }

        internal bool DoLimitedStretchHands => this.LimitedStretchHands > 0;

        internal bool DoLimitedStretchInvestment => this.LimitedStretchInvestments > 0;

        //internal bool DoAbandonAtOrUnderChainScore => this.AbandonAtOrUnderChainScore != null;

        //internal bool HasBadRunLimit => this.BadRunLimit > 0;

        //internal bool HasGoodRunLimit => this.GoodRunLimit > 0;

        //internal bool DoStopAtMinusUnits => this.StopAtMinusUnits > 0;

        internal bool DoHailMary => this.HailMaryMode != WalkHailMary.None;
    }
}
