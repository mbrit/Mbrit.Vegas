using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal interface IRng
    {
        int Next(int minInclusive, int maxInclusive);
    }
}
