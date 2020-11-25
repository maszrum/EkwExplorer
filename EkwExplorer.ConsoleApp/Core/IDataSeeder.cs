using System.Threading.Tasks;
using EkwExplorer.Models;

namespace EkwExplorer.Core
{
    internal interface IDataSeeder
    {
        Task SeedAsync(BookNumber fromNumber, BookNumber toNumber);
    }
}