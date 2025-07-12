using BootFX.Common;
using BootFX.Common.Services;
using System.Diagnostics;
using Mbrit.Vegas.Service;
using BootFX.Common.Data;
using System.Runtime.CompilerServices;
using Mbrit.Vegas.Simulator;

namespace Mbrit.Vegas.Utility
{
    internal class Program
    {
        private static VegasService Service { get; set; }

        static void Main(string[] args)
        {
            try
            {
                using (VegasRuntime.Start(Module.Utility))
                {
                    CliCommand.Loop((name, command) =>
                    {
                        if (name == "bfx")
                            new BfxCli().Run();
                        else if (name == "ud")
                            VegasRuntime.UpdateDatabase();
                        else if (name == "cd")
                            DatabaseUpdate.Current.CheckAndDump();
                        else if (name == "service")
                        {
                            if (Service != null)
                                throw new InvalidOperationException("Service already running.");

                            Service = new VegasService(new CancellationTokenSource().Token);
                            Service.Start();
                        }
                        else if (name == "jack")
                            new BlackjackFoo().DoMagic();
                        else if (name == "rand")
                            new RandomFoo().DoMagic();
                        else if (name == "uth")
                            new UthFoo().DoMagic();
                        else if (name == "walk")
                            new WalkFoo().DoMagic();
                        else if (name == "walkn")
                            new WalkFoo().DoMagicN();
                        //else if (name == "walk2")
                        //    new WalkSweep().DoMagic();
                        else if (name == "craps")
                            new CrapsFoo().DoMagic();
                        else if (name == "all")
                            new UniversalFoo().DoMagic();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (Debugger.IsAttached)
                    Console.ReadLine();
            }
        }
    }
}
