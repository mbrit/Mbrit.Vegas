using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class UthArgs : ISimulatorArgs
    {
        internal int Ante { get; set; }
        internal decimal WinTargetWeight { get; set; }
    }
}
