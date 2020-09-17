using System;
using System.Threading.Tasks;
using EkwClicker.Core;
using EkwClicker.Models;

namespace EkwClicker.Datasource.Repositories
{
    internal class BooksRepository : IBooksRepository
    {
        public Task AddAsync(BookInfo bookInfo)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetEmptyRandomIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}