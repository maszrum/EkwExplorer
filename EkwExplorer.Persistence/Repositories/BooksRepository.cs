using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EkwExplorer.Core;
using EkwExplorer.Core.Models;
using EkwExplorer.Persistence.Entities;
using EkwExplorer.Persistence.Mappers;

namespace EkwExplorer.Persistence.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        public BooksRepository(IDbAccess access)
        {
            _db = access ?? throw new ArgumentNullException(nameof(access));
        }

        private readonly IDbAccess _db;

        protected ISqlQueries Queries => _db.Queries;

        public async Task AddBookAsync(BookInfo bookInfo)
        {
            var query = Queries.AddBook;
            var entity = new BookToEntityMapper(bookInfo).MapBook();
            
            await _db.Db.ExecuteAsync(query, entity);
        }

        public async Task UpdateBookAsync(BookInfo bookInfo)
        {
            var query = Queries.UpdateBook;

            var entity = new BookToEntityMapper(bookInfo).MapBook();

            var updatedRows = await _db.Db.ExecuteAsync(query, entity);

            if (updatedRows != 1)
            {
                throw new ArgumentException(
                    "book with specified id was not found", nameof(bookInfo));
            }
        }

        public async Task<BookInfo> GetRandomNotFilledBookAsync()
        {
            var query = Queries.GetRandomNotFilledBook;
            var entity = await _db.Db.QuerySingleOrDefaultAsync<BookEntity>(query);
            
            if (entity == null)
            {
                throw new InvalidOperationException(
                    "there are no empty books");
            }
            
            var model = new BookToModelMapper()
                .Map(entity)
                .Finish();
            
            return model;
        }

        public Task<bool> IsAnyNotFilled()
        {
            var query = Queries.IsAnyNotFilled;

            return _db.Db.ExecuteScalarAsync<bool>(query);
        }

        public async Task AddPropertyFromBookAsync(BookInfo bookInfo)
        {
            var entities = new BookToEntityMapper(bookInfo)
                .MapPropertyNumbers()
                .ToList();

            if (entities.Count > 0)
            {
                var query = Queries.AddProperty;

                var transaction = await _db.Db.BeginTransactionAsync();

                await _db.Db.ExecuteAsync(query, entities);

                await transaction.CommitAsync();
            }
        }
    }
}