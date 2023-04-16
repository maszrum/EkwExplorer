using EkwExplorer.Core.Models;

namespace EkwExplorer.Core;

public interface IDataSeeder
{
    Task SeedAsync(BookNumber fromNumber, BookNumber toNumber);
}
