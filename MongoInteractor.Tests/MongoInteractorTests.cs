using DotNetEnv;

namespace MongoInteractions.Tests
{
    [TestClass]
    public class MongoInteractorTests
    {
        public MongoInteractor MongoInteractor { get; private set; }

        public MongoInteractorTests()
        {
            Env.Load(Path.Join(GetProjectDirectory(), ".env"));
            MongoInteractor = new MongoInteractor(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        }

        [TestMethod]
        public void CanStart()
        {
            MongoInteractor.Start();
            Assert.IsTrue(MongoInteractor.IsReady);
        }

        [TestMethod]
        public void GetsDatabase()
        {
            if (!MongoInteractor.IsReady) MongoInteractor.Start();

            var database = MongoInteractor.GetDatabase("test");
            Assert.IsNotNull(database);
        }

        [TestMethod]
        public void DropsDatabase()
        {
            if (!MongoInteractor.IsReady) MongoInteractor.Start();

            MongoInteractor.DropDatabase("irrelevant");
        }

        public static string GetProjectDirectory()
        {
            var cwd = Directory.GetCurrentDirectory();
            string projectDirectory;

            if (cwd.Contains("bin"))
            {
                projectDirectory = Directory.GetParent(cwd)!.Parent!.Parent!.FullName;
            }
            else
            {
                projectDirectory = cwd;
            }

            return projectDirectory;
        }
    }
}
