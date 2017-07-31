using System;
using NuGet.Common;
using NuKeeper.Configuration;
using NuKeeper.Engine;
using NuKeeper.Github;
using NuKeeper.NuGet.Api;
using NuKeeper.ProcessRunner;
using NuKeeper.RepositoryInspection;
using SimpleInjector;

namespace NuKeeper
{
    public class Program
    {
        public static int Main(string[] args)
        {
            TempFiles.DeleteExistingTempDirs();
                
            var settings = SettingsParser.ReadSettings(args);

            if(settings == null)
            {
                Console.WriteLine("Exiting early...");
                return 1;
            }

            var container = RegisterContainer(settings);

            TempFiles.DeleteExistingTempDirs();

            // get some storage space
            var tempDir = TempFiles.MakeUniqueTemporaryPath();

            var engine = container.GetInstance<IEngine>();

            engine.RunAll(tempDir, container.GetInstance<Settings>().GithubToken)
                .GetAwaiter().GetResult();

            return 0;
        }

        private static Container RegisterContainer(Settings settings)
        {
            var container = new Container();

            container.Register(() => settings, Lifestyle.Singleton);
            container.Register<IGithub, OctokitClient>();
            container.Register<IGithubRepositoryDiscovery, GithubRepositoryDiscovery>();
            container.Register<IPackageUpdateSelection, PackageUpdateSelection>();
            container.Register<IPackageUpdatesLookup, PackageUpdatesLookup>();
            container.Register<IBulkPackageLookup, BulkPackageLookup>();
            container.Register<IApiPackageLookup, ApiPackageLookup>();
            container.Register<IEngine, Engine.Engine>();
            container.Register<IRepositoryScanner, RepositoryScanner>();
            container.Register<ILogger, ConsoleLogger>();
            container.Register<IExternalProcess, ExternalProcess>();

            return container;
        }
    }
}
