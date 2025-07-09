using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public class Chain
    {
        public int Steps { get; private set; }

        public void Increment() => this.Steps++;

        public void Reset() => this.Steps = 0;
    }
}
