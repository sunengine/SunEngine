using System.Diagnostics;
using System.IO;

namespace SunEngine
{
    public class ProjectBuild
    {
        private readonly string solutionDirectory;
        private readonly string clientDirectory;


        public ProjectBuild(string solutionDirectory, string clientDirectory)
        {
            this.solutionDirectory = solutionDirectory;
            this.clientDirectory = clientDirectory;
        }

        public void Build()
        {
            RemoveOldBuildDir();
            var slnPath = Path.Combine(solutionDirectory, "SunEngine.sln");
            Process.Start($"dotnet publish -c Release \"{slnPath}\" -o \"/build\" -v m");
        }

        void RemoveOldBuildDir()
        {
            var buildPath = Path.Combine(solutionDirectory, "build");
            Directory.Delete(buildPath, true);
        }
    }
}