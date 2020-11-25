using System.Threading.Tasks;
using EkwExplorer.Core.Models;

namespace EkwExplorer.Core
{
    public interface IBooksRepository
    {
        Task AddBookAsync(BookInfo bookInfo);
        Task UpdateBookAsync(BookInfo bookInfo);
        Task AddPropertyFromBookAsync(BookInfo bookInfo);
        Task<bool> IsAnyNotFilled();
        Task<BookInfo> GetRandomNotFilledBookAsync();
    }
}