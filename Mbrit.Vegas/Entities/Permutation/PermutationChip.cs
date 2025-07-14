using BootFX.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    internal class PermutationChip : IPermutation
    {
        public int PermutationId { get; }

        public bool MajorBustSeen { get; }
        public int MajorBustHand { get; }
        public decimal MajorBustEv { get; }

        public bool MinorBustSeen { get; }
        public int MinorBustHand { get; }
        public decimal MinorBustEv { get; }

        public bool Spike0p5Seen { get; }
        public int Spike0p5Hand { get; }
        public decimal Spike0p5Ev { get; }

        public bool Spike1Seen { get; }
        public int Spike1Hand { get; }
        public decimal Spike1Ev { get; }

        public decimal ResolvedScore { get; }

        internal PermutationChip(DataRow row)
        {
            this.PermutationId = row.GetValue<int>("PermutationId");

            this.MajorBustSeen = row.GetValue<bool>("MajorBustSeen");
            this.MajorBustHand = row.GetValue<int>("MajorBustHand");
            this.MajorBustEv = row.GetValue<decimal>("MajorBustEv");

            this.MinorBustSeen = row.GetValue<bool>("MinorBustSeen");
            this.MinorBustHand = row.GetValue<int>("MinorBustHand");
            this.MinorBustEv = row.GetValue<decimal>("MinorBustEv");

            this.Spike0p5Seen = row.GetValue<bool>("Spike0p5Seen");
            this.Spike0p5Hand = row.GetValue<int>("Spike0p5Hand");
            this.Spike0p5Ev = row.GetValue<decimal>("Spike0p5Ev");

            this.Spike1Seen = row.GetValue<bool>("Spike1Seen");
            this.Spike1Hand = row.GetValue<int>("Spike1Hand");
            this.Spike1Ev = row.GetValue<decimal>("Spike1Ev");

            var score = row.GetValue<decimal>("Score");
            this.ResolvedScore = score / Permutation.ScoreAdjust;
        }
    }
}
