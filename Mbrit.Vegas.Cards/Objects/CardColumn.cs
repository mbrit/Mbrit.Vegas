using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards
{
    internal class CardColumn
    {
        internal float Width { get; set; }
        internal string Label { get; }
        internal bool HasSeparator { get; set; } = true;

        internal CardColumn(string label)
        {
            this.Label = label;
        }
    }
}
