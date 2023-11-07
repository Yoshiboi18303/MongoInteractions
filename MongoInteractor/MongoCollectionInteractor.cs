using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoInteractions
{
    /// <summary>
    /// Represents an interactor for interacting with a collection in a database
    /// </summary>
    /// <typeparam name="TDocument">A type that represents what the data in the collection looks like</typeparam>
    public class MongoCollectionInteractor<TDocument>
    {
        /// <summary>
        /// Gets the base <see cref="MongoClient" /> for this <see cref="MongoCollectionInteractor{TDocument}"/>
        /// </summary>
        public MongoClient Client { get; private set; }
        /// <summary>
        /// Gets the <see cref="IMongoCollection{TDocument}"/> that this <see cref="MongoCollectionInteractor{TDocument}"/> interacts with.
        /// </summary>
        public IMongoCollection<TDocument> Collection { get; private set; }
        /// <summary>
        /// Gets the parent <see cref="MongoDatabaseInteractor"/> this <see cref="MongoCollectionInteractor{TDocument}"/> is in.
        /// </summary>
        public MongoDatabaseInteractor Database { get; private set; }
        /// <summary>
        /// Gets the base <see cref="MongoInteractor"/> this <see cref="MongoCollectionInteractor{TDocument}"/> uses.
        /// </summary>
        public MongoInteractor Mongo { get; private set; }

        internal MongoCollectionInteractor(MongoClient client, IMongoCollection<TDocument> collection, MongoDatabaseInteractor database, MongoInteractor mongo)
        {
            Client = client;
            Collection = collection;
            Database = database;
            Mongo = mongo;
        }

        /// <summary>
        /// Finds all documents that match a filter, then returns an <see cref="IFindFluent{TDocument, TProjection}"/>
        /// </summary>
        /// <param name="filter">The <see cref="FilterDefinition{TDocument}"/> to use</param>
        /// <param name="options">The options to use for this find operation</param>
        public IFindFluent<TDocument, TDocument> Find(FilterDefinition<TDocument> filter, FindOptions? options = null)
        {
            return Collection.Find(filter, options);
        }

        /// <summary>
        /// Gets all documents in the collection and returns a List with all entries.
        /// </summary>
        /// <param name="options">The options to use for this find operation</param>
        public List<TDocument> Find(FindOptions? options = null)
        {
            var found = Collection.Find(null, options);
            return found.ToList();
        }

        /// <summary>
        /// Asynchronously finds all documents that match a filter, then returns an <see cref="IAsyncCursor{TDocument}"/>
        /// </summary>
        /// <param name="filter">The <see cref="FilterDefinition{TDocument}"/> to use</param>
        /// <param name="options">The options to use for this find operation</param>
        public async Task<IAsyncCursor<TDocument>> FindAsync(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument>? options = null)
        {
            return await Collection.FindAsync(filter, options);
        }

        /// <summary>
        /// Asynchronously gets all documents in the collection and returns a List with all entries.
        /// </summary>
        /// <param name="options">The options to use for this find operation</param>
        public async Task<List<TDocument>> FindAsync(FindOptions<TDocument, TDocument>? options = null)
        {
            var found = await Collection.FindAsync(null, options);
            return found.ToList();
        }

        /// <summary>
        /// Finds one document from the collection.
        /// </summary>
        /// <param name="filter">The <see cref="FilterDefinition{TDocument}"/> to use</param>
        /// <param name="options">The options to use for this find operation</param>
        public TDocument FindOne(FilterDefinition<TDocument> filter, FindOptions? options = null)
        {
            var found = Find(filter, options);
            return found.Single();
        }

        /// <summary>
        /// Asynchronously finds one document from the collection.
        /// </summary>
        /// <param name="filter">The <see cref="FilterDefinition{TDocument}"/> to use</param>
        /// <param name="options">The options to use for this find operation</param>
        public async Task<TDocument> FindOneAsync(FilterDefinition<TDocument> filter, FindOptions<TDocument, TDocument>? options = null)
        {
            var cursor = await FindAsync(filter, options);
            return cursor.Single();
        }

        /// <summary>
        /// Finds a document from the provided filter and updates it with the provided update definition
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <param name="update">The update definition to use</param>
        /// <param name="options">The options to use</param>
        /// <returns>The updated document</returns>
        public TDocument FindOneAndUpdate(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument>? options = null)
        {
            return Collection.FindOneAndUpdate(filter, update, options);
        }

        /// <summary>
        /// Asynchronously finds a document from the provided filter and updates it with the provided update definition
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <param name="update">The update definition to use</param>
        /// <param name="options">The options to use</param>
        /// <returns>A Task with the updated document</returns>
        public async Task<TDocument> FindOneAndUpdateAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument>? options = null)
        {
            return await Collection.FindOneAndUpdateAsync(filter, update, options);
        }

        /// <summary>
        /// Finds a document from the provided filter and deletes it.
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <param name="options">The options to use</param>
        /// <returns>The deleted document</returns>
        public TDocument FindOneAndDelete(FilterDefinition<TDocument> filter, FindOneAndDeleteOptions<TDocument, TDocument>? options = null)
        {
            return Collection.FindOneAndDelete(filter, options);
        }

        /// <summary>
        /// Asynchronously finds a document with the provided filter and deletes it.
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <param name="options">The options to use</param>
        /// <returns>A Task with the deleted document</returns>
        public async Task<TDocument> FindOneAndDeleteAsync(FilterDefinition<TDocument> filter, FindOneAndDeleteOptions<TDocument, TDocument>? options = null)
        {
            return await Collection.FindOneAndDeleteAsync(filter, options);
        }

        /// <summary>
        /// Finds a document with the provided filter and replaces it with the provided replacement.
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <param name="replacement">The brand new document to replace the current document with</param>
        /// <param name="options">The options to use</param>
        /// <returns>The replaced document</returns>
        public TDocument FindOneAndReplace(FilterDefinition<TDocument> filter, TDocument replacement, FindOneAndReplaceOptions<TDocument, TDocument>? options = null)
        {
            return Collection.FindOneAndReplace(filter, replacement, options);
        }

        /// <summary>
        /// Asynchronously finds a document with the provided filter and replaces it with the provided replacement.
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <param name="replacement">The brand new document to replace the current document with</param>
        /// <param name="options">The options to use</param>
        /// <returns>A Task with the replaced document</returns>
        public async Task<TDocument> FindOneAndReplaceAsync(FilterDefinition<TDocument> filter, TDocument replacement, FindOneAndReplaceOptions<TDocument, TDocument>? options = null)
        {
            return await Collection.FindOneAndReplaceAsync(filter, replacement, options);
        }

        /// <summary>
        /// Inserts many documents into the collection
        /// </summary>
        /// <param name="documents">An <see cref="IEnumerable{T}"/> with all the documents to insert</param>
        /// <param name="options">The options to use</param>
        public void InsertMany(IEnumerable<TDocument> documents, InsertManyOptions? options = null)
        {
            Collection.InsertMany(documents, options);
        }

        /// <summary>
        /// Asynchronously inserts many documents into the collection
        /// </summary>
        /// <param name="documents">An <see cref="IEnumerable{TDocument}"/> with all the documents to insert</param>
        /// <param name="options">The options to use</param>
        public async Task InsertManyAsync(IEnumerable<TDocument> documents, InsertManyOptions? options = null)
        {
            await Collection.InsertManyAsync(documents, options);
        }

        /// <summary>
        /// Inserts one document into the collection
        /// </summary>
        /// <param name="document">The document to insert</param>
        /// <param name="options">The options to use</param>
        public void InsertOne(TDocument document, InsertOneOptions? options = null)
        {
            Collection.InsertOne(document, options);
        }

        /// <summary>
        /// Asynchronously inserts one document into the collection
        /// </summary>
        /// <param name="document">The document to insert</param>
        /// <param name="options">The options to use</param>
        public async Task InsertOneAsync(TDocument document, InsertOneOptions? options = null)
        {
            await Collection.InsertOneAsync(document, options);
        }
    }
}
