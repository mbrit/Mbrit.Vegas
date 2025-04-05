using BootFX.Common;
using Mbrit.Vegas.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Mbrit.Vegas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var asService = CommandLinePropertyBag.Current.GetBooleanValue("asService", false);
                if (asService)
                {
                    var builder = Host.CreateDefaultBuilder(args);
                    builder.UseWindowsService();
                    builder.ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<ServiceShim>();
                    });

                    var host = builder.Build();
                    host.Run();
                }
                else
                {
                    var task = StartServiceTask(new CancellationTokenSource().Token);
                    task.Wait();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                if (Debugger.IsAttached)
                    Console.ReadLine();
            }
        }

        internal static async Task StartServiceTask(CancellationToken token)
        {
            using (VegasRuntime.Start(Module.Console))
            {
                var service = new VegasService(token);
                service.Start();

                await Task.Run(() =>
                {
                    Runtime.Current.WaitForUserExit(token);
                });
            }
        }
    }
}
