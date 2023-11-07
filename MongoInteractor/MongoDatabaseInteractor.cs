using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoInteractions
{
    /// <summary>
    /// Represents an interactor that interacts with a database in a cluster.
    /// </summary>
    public class MongoDatabaseInteractor
    {
        /// <summary>
        /// Gets the <see cref="MongoClient"/> this <see cref="MongoDatabaseInteractor"/> uses.
        /// </summary>
        public MongoClient Client { get; private set; }
        /// <summary>
        /// Gets the <see cref="IMongoDatabase"/> this <see cref="MongoDatabaseInteractor"/> interacts with.
        /// </summary>
        public IMongoDatabase Database { get; private set; }
        /// <summary>
        /// Gets the <see cref="MongoInteractor"/> this <see cref="MongoDatabaseInteractor"/> is associated with.
        /// </summary>
        public MongoInteractor Mongo { get; private set; }

        internal MongoDatabaseInteractor(MongoClient client, IMongoDatabase database, MongoInteractor mongo)
        {
            Client = client;
            Database = database;
            Mongo = mongo;
        }

        /// <summary>
        /// Creates a new collection with the provided name and options.
        /// </summary>
        /// <param name="collectionName">The name for the collection</param>
        /// <param name="options">The options to use</param>
        public void CreateCollection(string collectionName, CreateCollectionOptions? options = null)
        {
            Database.CreateCollection(collectionName, options);
        }

        /// <summary>
        /// Asynchronously creates a new collection with the provided name and options
        /// </summary>
        /// <param name="collectionName">The name for the collection</param>
        /// <param name="options">The options to use</param>
        public async Task CreateCollectionAsync(string collectionName, CreateCollectionOptions? options = null)
        {
            await Database.CreateCollectionAsync(collectionName, options);
        }

        /// <summary>
        /// Drops (deletes) a collection, deleting all entries.
        /// </summary>
        /// <param name="collectionName">The name of the collection</param>
        /// <remarks>THIS CANNOT BE UNDONE ONCE EXECUTED, BE CAREFUL!</remarks>
        public void DropCollection(string collectionName)
        {
            Database.DropCollection(collectionName);
        }

        /// <summary>
        /// Asynchronously drops (deletes) a collection, deleting all entries.
        /// </summary>
        /// <param name="collectionName">The name of the collection</param>
        /// <remarks>THIS CANNOT BE UNDONE ONCE EXECUTED, BE CAREFUL!</remarks>
        public async Task DropCollectionAsync(string collectionName)
        {
            await Database.DropCollectionAsync(collectionName);
        }

        /// <summary>
        /// Gets a collection from the provided name, then returns a new <see cref="MongoCollectionInteractor{TDocument}"/>.
        /// </summary>
        /// <typeparam name="TDocument">A type that represents what the data in the collection looks like.</typeparam>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A <see cref="MongoCollectionInteractor{TDocument}"/> to allow for interactions with the collection.</returns>
        public MongoCollectionInteractor<TDocument> GetCollection<TDocument>(string collectionName, MongoCollectionSettings? settings = null)
        {
            var collection = Database.GetCollection<TDocument>(collectionName, settings);
            return new MongoCollectionInteractor<TDocument>(Client, collection, this, Mongo);
        }
    }
}
