using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EkwExplorer.Core;
using EkwExplorer.Core.Models;
using EkwExplorer.Datasource.Entities;
using EkwExplorer.Datasource.Mappers;

namespace EkwExplorer.Datasource.Repositories
{
    internal class BooksRepository : IBooksRepository
    {
        public BooksRepository(IDbAccess access, ISqlQueries queries)
        {
            _queries = queries;
            _db = access ?? throw new ArgumentNullException(nameof(access));
        }

        private readonly IDbAccess _db;
        private readonly ISqlQueries _queries;

        public async Task AddBookAsync(BookInfo bookInfo)
        {
            var query = _queries.AddBook;
            var entity = new BookToEntityMapper(bookInfo).MapBook();
            
            await _db.Db.ExecuteAsync(query, entity);
        }

        public async Task UpdateBookAsync(BookInfo bookInfo)
        {
            var query = _queries.UpdateBook;

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
            var query = _queries.GetRandomNotFilledBook;
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
            var query = _queries.IsAnyNotFilled;

            return _db.Db.ExecuteScalarAsync<bool>(query);
        }

        public async Task AddPropertyFromBookAsync(BookInfo bookInfo)
        {
            var entities = new BookToEntityMapper(bookInfo)
                .MapPropertyNumbers()
                .ToList();

            if (entities.Count > 0)
            {
                var query = _queries.AddProperty;

                var transaction = await _db.Db.BeginTransactionAsync();

                await _db.Db.ExecuteAsync(query, entities);

                await transaction.CommitAsync();
            }
        }
    }
}