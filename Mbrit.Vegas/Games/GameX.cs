using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public abstract class GameX
    {
        public string Name { get; }

        protected GameX(string name)
        {
            this.Name = name;
        }
    }
}
