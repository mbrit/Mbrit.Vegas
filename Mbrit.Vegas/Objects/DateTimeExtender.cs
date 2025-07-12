using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public static class DateTimeExtender
    {
        public static DateTime GetLocalTime(DateTime dtUtc, int minutes) => dtUtc.AddMinutes(minutes); 
    }
}
