using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    [Flags]
    public enum RoundsBucketFlags
    {
        Burning = 0,
        Exact = 1,

        Default = Burning
    }
}
