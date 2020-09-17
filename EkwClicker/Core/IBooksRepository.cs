using System.Threading.Tasks;
using EkwClicker.Models;

namespace EkwClicker.Core
{
	internal interface IBooksRepository
    {
        Task<BookInfo> GetRandomNotFilledBookAsync();
        Task AddAsync(BookInfo bookInfo);
    }
}