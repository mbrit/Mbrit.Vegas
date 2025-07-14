using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public interface IPermutation
    {
        int PermutationId { get; }

        bool MajorBustSeen { get; }
        int MajorBustHand { get; }
        decimal MajorBustEv { get; }

        bool MinorBustSeen { get; }
        int MinorBustHand { get; }
        decimal MinorBustEv { get; }

        bool Spike0p5Seen { get; }
        int Spike0p5Hand { get; }
        decimal Spike0p5Ev { get; }

        bool Spike1Seen { get; }
        int Spike1Hand { get; }
        decimal Spike1Ev { get; }

        decimal ResolvedScore { get; }
    }
}
