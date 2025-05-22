using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal interface ISimulatorStep : IEnumerable<object>
    {
        PropertyInfo GetProperty();
    }
}
