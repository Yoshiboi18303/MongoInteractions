using DotNetEnv;
using MongoDB.Driver;

namespace MongoInteractions.Tests
{
    [TestClass]
    public class MongoCollectionInteractorTests
    {
        public MongoInteractor MongoInteractor { get; private set; }
        public MongoDatabaseInteractor DatabaseInteractor { get; private set; }
        public MongoCollectionInteractor<User> CollectionInteractor { get; private set; }
        public List<User> MockData { get; } = new List<User>()
        {
            new User()
            {
                Id = 1,
                Name = "John Doe",
                Email = "johndoe@example.com",
                Password = "password"
            },
            new User()
            {
                Id = 2,
                Name = "Jane Doe",
                Email = "jane@anothersite.net",
                Password = "youshallnotpass"
            }
        };

        public MongoCollectionInteractorTests()
        {
            Env.Load(Path.Join(MongoInteractorTests.GetProjectDirectory(), ".env"));
            MongoInteractor = new MongoInteractor(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            if (!MongoInteractor.IsReady) MongoInteractor.Start();
            DatabaseInteractor = MongoInteractor.GetDatabase("test");
            CollectionInteractor = DatabaseInteractor.GetCollection<User>("users");
        }

        [TestMethod]
        public void InsertsDocument()
        {
            var document = MockData[0];
            CollectionInteractor.InsertOne(document);

            Assert.IsNotNull(CollectionInteractor.FindOne(new FilterDefinitionBuilder<User>().Eq((user) => user.Id, document.Id)));
        }

        [TestMethod]
        public void DeletesDocument()
        {
            var document = MockData[0];
            var filter = new FilterDefinitionBuilder<User>().Eq((user) => user.Id, document.Id);
            CollectionInteractor.FindOneAndDelete(filter);

            Assert.ThrowsException<InvalidOperationException>(() => CollectionInteractor.FindOne(filter));
        }

        [TestMethod]
        public void InsertsManyDocuments()
        {
            CollectionInteractor.InsertMany(MockData);
            var documents = CollectionInteractor.Find();
            Assert.AreEqual(MockData.Count, documents.Count);
        }

        [TestMethod]
        public void UpdatesDocument()
        {
            var document = MockData[0];
            string newPassword = "br4nd1$@n3w&p4ssw0rd!!!";
            var filter = new FilterDefinitionBuilder<User>().Eq((user) => user.Id, document.Id);

            CollectionInteractor.FindOneAndUpdate(
                filter,
                new UpdateDefinitionBuilder<User>().Set((user) => user.Password, newPassword)
            );

            var updated = CollectionInteractor.FindOne(filter);
            Assert.AreEqual(newPassword, updated.Password);
        }

        [TestMethod]
        public void ReplacesDocument()
        {
            var document = MockData[1];
            var replacement = new User()
            {
                Id = document.Id,
                Name = "Loving Wife",
                Email = document.Email,
                Password = "i$$$4m$$$c0mpl3t3ly$$$s4f3"
            };
            var filter = new FilterDefinitionBuilder<User>().Eq((user) => user.Id, document.Id);

            // Replacement logic
            CollectionInteractor.FindOneAndReplace(filter, replacement);
            var replaced = CollectionInteractor.FindOne(filter);

            // Assert
            Assert.AreEqual(replacement.Password, replaced.Password);
            Assert.AreEqual(replacement.Name, replaced.Name);

            // Ensure email wasn't changed in the process
            Assert.AreEqual(replacement.Email, replaced.Email);
        }
    }
}
