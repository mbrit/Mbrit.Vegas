using BootFX.Common.Web;

namespace Mbrit.Vegas.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            using (VegasRuntime.Start(Module.WebApi))
            {
                using (WebRuntime.Start(new WebRuntimeArgs(new ApiPermissionsShim())))
                {
                    // for debugging, this allows any CORS origin and also allows use of plain HTTP...
                    var webArgs = new WebApiSetupArgs()
                    {
                        IsCorsOriginAllowed = (uri) => true,
                        UpgradeInsecureRequests = false,
                        //ApiExceptionHandler = new ApiErrorHandler()
                    };
                    webArgs.ConfigureDetailedErrorsForBlazor();

                    WebRuntime.InitializeWebApi(builder, (services) =>
                    {

                    }, (app) =>
                    {

                        Task.Run(() =>
                        {
                            PermutationCache.InitializeDefaultCache();
                        });

                    }, webArgs);
                }
            }
        }
    }
}