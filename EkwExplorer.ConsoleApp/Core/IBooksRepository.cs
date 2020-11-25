using System.Threading.Tasks;
using EkwExplorer.Models;

namespace EkwExplorer.Core
{
    internal interface IBooksRepository
    {
        Task AddBookAsync(BookInfo bookInfo);
        Task UpdateBookAsync(BookInfo bookInfo);
        Task AddPropertyFromBookAsync(BookInfo bookInfo);
        Task<bool> IsAnyNotFilled();
        Task<BookInfo> GetRandomNotFilledBookAsync();
    }
}