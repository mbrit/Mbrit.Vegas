using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public class GameOdds
    {
        public string Name { get; }
        private int X { get; }
        private int Y { get; }

        internal GameOdds(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

        public override string ToString() => $"{this.Name} ({this.X}:{this.Y})";

        public decimal GetWin(decimal bet) => bet * ((decimal)this.X / (decimal)this.Y);
    }
}
