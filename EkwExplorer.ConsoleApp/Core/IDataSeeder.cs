using System.Threading.Tasks;
using EkwClicker.Models;

namespace EkwClicker.Core
{
    internal interface IDataSeeder
    {
        Task SeedAsync(BookNumber fromNumber, BookNumber toNumber);
    }
}