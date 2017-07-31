using System.Threading.Tasks;

namespace NuKeeper.Engine
{
    public interface IEngine
    {
        Task RunAll(string tempDir, string githubToken);
    }
}