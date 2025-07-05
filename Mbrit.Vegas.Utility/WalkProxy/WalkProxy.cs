using BootFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WalkProxy : RestProxy
    {
        internal WalkProxy()
            : base("http://localhost:5000/")
        {
        }

        internal PredictResponse Predict(WalkOutcome outcome, IEnumerable<WinLoseDrawType> vectors)
        {
            var asArray = new List<int>();
            foreach (var vector in vectors)
            {
                if (vector == WinLoseDrawType.Win)
                    asArray.Add(1);
                else if (vector == WinLoseDrawType.Lose)
                    asArray.Add(0);
                else
                    throw new NotSupportedException($"Cannot handle '{vector}'.");
            }

            var label = 0;
            /*
            if (outcome == WalkOutcome.MajorBust)
                label = 0;
            else if (outcome == WalkOutcome.MinorBust)
                label = 1;
            else if (outcome == WalkOutcome.Evens)
                label = 2;
            else if (outcome == WalkOutcome.Minor)
                label = 3;
            else if (outcome == WalkOutcome.Spike)
                label = 4;
            else
                throw new NotSupportedException($"Cannot handle '{outcome}'.");
            */

            if (outcome == WalkOutcome.MajorBust)
                label = 0;
            else if (outcome == WalkOutcome.Minor)
                label = 2;
            else if (outcome == WalkOutcome.Spike)
                label = 3;
            else
            {
                // have to redo all this because we changed the labels...
                throw new NotImplementedException("This operation has not been implemented.");
            }

            return this.PostJson<PredictRequest, PredictResponse>("/predict", new PredictRequest()
            {
                Label = label,
                Vector = asArray,
            });
        }  
    }
}
