using System;
using System.Threading.Tasks;
using EkwClicker.Models;

namespace EkwClicker.Core
{
	internal interface IBooksRepository
    {
        Task<Guid> GetEmptyRandomIdAsync();
        Task AddAsync(BookInfo bookInfo);
    }
}