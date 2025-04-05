using BootFX.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Service
{
    public class VegasService : ServiceHost
    {
        public VegasService(CancellationToken token)
            : base(token)
        {
        }
    }
}
