using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EkwClicker.Core;
using EkwClicker.Datasource.Entities;
using EkwClicker.Datasource.Mappers;
using EkwClicker.Models;

namespace EkwClicker.Datasource.Repositories
{
    internal class BooksRepository : IBooksRepository
    {
        private readonly DbConnection _connection;

        public BooksRepository(DbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task AddBookAsync(BookInfo bookInfo)
        {
            var query = SqlQueries.AddBook;
            var entity = new BookToEntityMapper(bookInfo).MapBook();
            
            await _connection.Db.ExecuteAsync(query, entity);
        }

        public async Task UpdateBookAsync(BookInfo bookInfo)
        {
            var query = SqlQueries.UpdateBook;

            var entity = new BookToEntityMapper(bookInfo).MapBook();

            var updatedRows = await _connection.Db.ExecuteAsync(query, entity);

            if (updatedRows != 1)
            {
                throw new ArgumentException(
                    "book with specified id was not found", nameof(bookInfo));
            }
        }

        public async Task<BookInfo> GetRandomNotFilledBookAsync()
        {
            var query = SqlQueries.GetRandomNotFilledBook;
            var entity = await _connection.Db.QuerySingleOrDefaultAsync<BookEntity>(query);
            
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
            var query = SqlQueries.IsAnyNotFilled;

            return _connection.Db.ExecuteScalarAsync<bool>(query);
        }

        public async Task AddPropertyFromBookAsync(BookInfo bookInfo)
        {
            var entities = new BookToEntityMapper(bookInfo)
                .MapPropertyNumbers()
                .ToList();

            if (entities.Count > 0)
            {
                var query = SqlQueries.AddProperty;

                var transaction = await _connection.Db.BeginTransactionAsync();

                await _connection.Db.ExecuteAsync(query, entities);

                await transaction.CommitAsync();
            }
        }
    }
}