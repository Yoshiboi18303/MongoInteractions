using DotNetEnv;
using MongoInteractions;

namespace TestProject
{
    public class Program
    {
        public static void Main()
        {
            var program = new Program();

            // Load the ".env" file
            Env.Load(Path.Join(program.GetProjectDirectory(), ".env"));

            program.MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            // Create an interactor
            var interactor = new MongoInteractor(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        }

        // Useful for Visual Studio
        private string GetProjectDirectory()
        {
            var cwd = Directory.GetCurrentDirectory();
            string projectDirectory;

            if (cwd.Contains("bin"))
            {
                projectDirectory = Directory.GetParent(cwd).Parent.Parent.FullName;
            }
            else
            {
                projectDirectory = cwd;
            }

            return projectDirectory;
        }
    }
}
