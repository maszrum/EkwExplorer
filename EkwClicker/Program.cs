using System;
using System.Threading.Tasks;
using EkwClicker.Algorithms;
using EkwClicker.Datasource;
using EkwClicker.Datasource.Repositories;
using EkwClicker.Models;
using EkwClicker.Seeker;

// ReSharper disable ClassNeverInstantiated.Global

namespace EkwClicker
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var availableDatabases = DbAccess.GetAvailableDatabases();
            if (availableDatabases.Count == 0)
                Console.WriteLine("No available databases");
            else
            {
                Console.WriteLine("Available databases:");
                Console.WriteLine(string.Join(Environment.NewLine, availableDatabases));
            }

            var input = ProgramInput.ReadFromConsole();
            
            await using var connection = DbAccess.Exists(input.DatabaseFile)
                ? await DbAccess.Connect(input.DatabaseFile)
                : await DbAccess.Create(input.DatabaseFile);

            var repository = new BooksRepository(connection);
            var seeder = new DatasourceSeeder(repository);

            if (input.NumberFrom.HasValue && input.NumberTo.HasValue)
            {
                var numberFrom = new BookNumber(input.CourtCode, input.NumberFrom.Value.ToString("D8"));
                var numberTo = new BookNumber(input.CourtCode, input.NumberTo.Value.ToString("D8"));
                await seeder.SeedAsync(numberFrom, numberTo);
            }

            using var clicker = new SeleniumClicker();
            var seeker = new BookInfoSeeker(clicker);

            clicker.GotoHome();
            await Task.Delay(1000);
            clicker.CloseCookiesInfo();

            for (int i = 1; i <= 10; i++)
            {
                var randomBook = await repository.GetRandomNotFilledBookAsync();

                var bookExists = seeker.ReadBookInfo(randomBook);
                if (!bookExists)
                {
                    BookInfo.MarkAsNotFound(randomBook);
                }
                else
                {
                    var properties = seeker.ReadProperties();
                    randomBook.AddNewProperties(properties);
                }

                await repository.UpdateBookAsync(randomBook);
                await repository.AddPropertyFromBookAsync(randomBook);

                clicker.BackToCriteria();
            }
        }
    }
}