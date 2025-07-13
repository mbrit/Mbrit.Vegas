using BootFX.Common;
using BootFX.Common.Crypto;
using BootFX.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public static class VegasRuntime
    {
        public static string Environment { get; private set; }

        public static IDisposable Start(Module module)
        {
            var disposer = Runtime.Start("Mbrit", "Vegas", module.ToString(), ProductVersion);
            Environment = Runtime.Current.GetLocalEnvironmentName();
            return disposer;
        }

        public static void UpdateDatabase()
        {
            DatabaseUpdate.Current.Update(null, new DatabaseUpdateArgs());
        }

        public static string GetToken() => EncryptionHelper.GetToken();

        public static Random GetRng() => new Random(GetToken().GetHashCode());

        public static Version ProductVersion => typeof(VegasRuntime).Assembly.GetName().Version;
    }
}
