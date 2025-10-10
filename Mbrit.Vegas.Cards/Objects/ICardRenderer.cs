using Mbrit.Vegas.Lens.Graph;
using Mbrit.Vegas.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards
{
    internal interface ICardRenderer
    {
        void Render(IGraphics graphics);
    }
}
