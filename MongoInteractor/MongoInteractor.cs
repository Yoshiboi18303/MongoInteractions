using MongoDB.Driver;
using MongoInteractor.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MongoInteractions
{
    /// <summary>
    /// Represents an interactor to interact with a MongoDB cluster.
    /// </summary>
    public class MongoInteractor
    {
        /// <summary>
        /// Gets the <see cref="MongoClient"/> this <see cref="MongoInteractor"/> uses.
        /// </summary>
        public MongoClient Client { get; private set; }
        /// <summary>
        /// Gets whether this <see cref="MongoInteractor"/> is ready or not.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="MongoInteractor"/>.
        /// </summary>
        public MongoInteractor()
        {
            Client = new MongoClient();
        }

        /// <summary>
        /// Initializes a new <see cref="MongoInteractor"/> with the provided <see cref="MongoClientSettings" />
        /// </summary>
        /// <param name="settings">The settings to use</param>
        public MongoInteractor(MongoClientSettings settings)
        {
            Client = new MongoClient(settings);
        }

        /// <summary>
        /// Initializes a new <see cref="MongoInteractor"/> with the provided <see cref="MongoUrl"/>
        /// </summary>
        /// <param name="url">The URL to use</param>
        public MongoInteractor(MongoUrl url)
        {
            Client = new MongoClient(url);
        }

        /// <summary>
        /// Initializes a new <see cref="MongoInteractor"/> with the provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public MongoInteractor(string connectionString)
        {
            Client = new MongoClient(connectionString);
        }

        private void EnsureReady()
        {
            if (!IsReady) throw new NotReadyException("The client has not started yet!");
        }

        /// <summary>
        /// Starts the <see cref="MongoInteractor"/> which is required before any interactions.
        /// </summary>
        /// <exception cref="Exception">Throws if the client has already started</exception>
        public IClientSessionHandle Start()
        {
            if (IsReady) throw new Exception("The client has already started!");

            IsReady = true;
            return Client.StartSession();
        }

        /// <summary>
        /// Asynchronously starts the <see cref="MongoInteractor"/> which is required before any interactions.
        /// </summary>
        /// <exception cref="Exception">Throws if the client has already started</exception>
        public async Task<IClientSessionHandle> StartAsync()
        {
            if (IsReady) throw new Exception("The client has already started!");

            IsReady = true;
            return await Client.StartSessionAsync();
        }

        /// <summary>
        /// Gets a database from your MongoDB cluster from the provided database name then returns a <see cref="MongoDatabaseInteractor"/>.
        /// </summary>
        /// <param name="databaseName">The database name to get.</param>
        /// <param name="settings">The <see cref="MongoDatabaseSettings"/> to use.</param>
        /// <returns>A <see cref="MongoDatabaseInteractor"/> to provide interaction methods for your database.</returns>
        /// <exception cref="NotReadyException" />
        public MongoDatabaseInteractor GetDatabase([NotNull] string databaseName, MongoDatabaseSettings? settings = null)
        {
            EnsureReady();

            var database = Client.GetDatabase(databaseName, settings);
            return new MongoDatabaseInteractor(Client, database, this);
        }

        /// <summary>
        /// Drops (deletes) a database from your cluster.
        /// </summary>
        /// <param name="databaseName">The database name to delete.</param>
        /// <exception cref="NotReadyException" />
        public void DropDatabase(string databaseName)
        {
            EnsureReady();

            Client.DropDatabase(databaseName);
        }

        /// <summary>
        /// Asynchronously drops (deletes) a database from your cluster.
        /// </summary>
        /// <param name="databaseName">The database name to delete.</param>
        /// <exception cref="NotReadyException" />
        public async Task DropDatabaseAsync(string databaseName)
        {
            EnsureReady();

            await Client.DropDatabaseAsync(databaseName);
        }
    }
}
