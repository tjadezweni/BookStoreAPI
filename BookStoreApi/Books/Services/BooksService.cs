using BookStoreApi.Books.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Books.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);
            _booksCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true)
                .ToListAsync();

        public async Task<Book?> GetByIdAsync(string id) =>
            await _booksCollection.Find(book => book.Id == id)
                .FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book newBook) =>
            await _booksCollection.ReplaceOneAsync(book => book.Id == id, newBook);

        public async Task DeleteAsync(string id) =>
            await _booksCollection.DeleteOneAsync(book => book.Id == id);
    }
}
