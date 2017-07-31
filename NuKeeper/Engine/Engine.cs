using System;
using System.Threading.Tasks;
using NuKeeper.Git;
using NuKeeper.Github;
using NuKeeper.NuGet.Api;
using NuKeeper.ProcessRunner;
using NuKeeper.RepositoryInspection;

namespace NuKeeper.Engine
{
    public class Engine : IEngine
    {
        private readonly IGithubRepositoryDiscovery _repositoryDiscovery;
        private readonly IPackageUpdatesLookup _updatesLookup;
        private readonly IPackageUpdateSelection _updateSelection;
        private readonly IGithub _github;
        private readonly IRepositoryScanner _repositoryScanner;
        private readonly IExternalProcess _externalProcess;

        public Engine(
            IGithubRepositoryDiscovery repositoryDiscovery,
            IPackageUpdatesLookup updatesLookup,
            IPackageUpdateSelection updateSelection,
            IGithub github,
            IRepositoryScanner repositoryScanner,
            IExternalProcess externalProcess)
        {
            _repositoryDiscovery = repositoryDiscovery;
            _updatesLookup = updatesLookup;
            _updateSelection = updateSelection;
            _github = github;
            _repositoryScanner = repositoryScanner;
            _externalProcess = externalProcess;
        }

        public async Task RunAll(string tempDir, string githubToken)
        {
            var githubUser = await _github.GetCurrentUser();
            Console.WriteLine($"Read github user '{githubUser}'");

            var git = new LibGit2SharpDriver(tempDir, githubUser, githubToken);

            var repositories = await _repositoryDiscovery.GetRepositories();

            foreach (var repository in repositories)
            {
                try
                {
                    var repositoryUpdater =
                        new RepositoryUpdater(_updatesLookup, _github, git, tempDir, _updateSelection, _repositoryScanner, _externalProcess, repository);
                    await repositoryUpdater.Run();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Repo failed {e.GetType().Name}: {e.Message}");
                }
            }
        }
    }
}
