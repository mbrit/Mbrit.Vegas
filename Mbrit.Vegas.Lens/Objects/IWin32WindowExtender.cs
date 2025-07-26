using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens
{
    internal static class IWin32WindowExtender
    {
        internal static void ReportExceptions(this IWin32Window window, Action callback)
        {
            try
            {
                callback();
            }
            catch(Exception ex)
            {
                window.ShowMessage(ex.ToString());
            }
        }

        internal static void ShowMessage(this IWin32Window window, string message) =>
            MessageBox.Show(window, message);
    }
}
