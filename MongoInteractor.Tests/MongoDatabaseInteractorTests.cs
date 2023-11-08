using DotNetEnv;

namespace MongoInteractions.Tests
{
    [TestClass]
    public class MongoDatabaseInteractorTests
    {
        public MongoInteractor MongoInteractor { get; private set; }
        public MongoDatabaseInteractor DatabaseInteractor { get; private set; }

        public MongoDatabaseInteractorTests()
        {
            Env.Load(Path.Join(MongoInteractorTests.GetProjectDirectory(), ".env"));
            MongoInteractor = new MongoInteractor(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            if (!MongoInteractor.IsReady) MongoInteractor.Start();
            DatabaseInteractor = MongoInteractor.GetDatabase("test");
        }

        [TestMethod]
        public void DropsCollection()
        {
            DatabaseInteractor.DropCollection("irrelevant-data");
        }

        [TestMethod]
        public void GetsUserCollection()
        {
            var collection = DatabaseInteractor.GetCollection<User>("users");
            Assert.IsNotNull(collection, "Failed to get collection");
        }
    }
}
