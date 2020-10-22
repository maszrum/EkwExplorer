using System;
using System.Threading.Tasks;
using EkwClicker.Algorithms;
using EkwClicker.Core;
using EkwClicker.Datasource;
using EkwClicker.Datasource.Repositories;
using EkwClicker.Models;
using EkwClicker.Seeker;

// ReSharper disable ClassNeverInstantiated.Global

namespace EkwClicker
{
    internal class Program
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private static readonly ChromeOptionsProvider OptionsProvider = new ChromeOptionsProvider();
        
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

            var seeker = await OpenSeeker();
            
            var captchaErrors = 0;
            var downloadedBooks = 0;
            
            while (true)
            {
                try
                {
                    await ExploringStep(seeker, repository);
                    downloadedBooks++;
                    captchaErrors = 0;
                }
                catch (Exception exception) when (exception.Message.Contains("captcha"))
                {
                    captchaErrors++;
                    if (captchaErrors > 5)
                    {
                        captchaErrors = 0;
                        
                        Console.WriteLine("Reopening chrome driver...");
                        seeker = await ReopenSeeker(seeker);
                        Console.WriteLine("Opened new istance of chrome driver");
                    }
                }
                catch (Exception ex) when (ex.GetType().Namespace == "OpenQA.Selenium")
                {
                    seeker = await ReopenSeeker(seeker);
                }
                catch (InvalidOperationException ioe) when (ioe.Message.Contains("was not read"))
                {
                    seeker = await ReopenSeeker(seeker);
                }
                
                Console.WriteLine($"Downloaded books: {downloadedBooks}");
            }
        }
        
        private static Task<BookInfoSeeker> OpenSeeker()
            => ReopenSeeker(null);
        
        private static async Task<BookInfoSeeker> ReopenSeeker(BookInfoSeeker seeker)
        {
            seeker?.Clicker.Dispose();
            
            var clicker = await CreateClicker(OptionsProvider);
            
            seeker = new BookInfoSeeker(clicker);
            return seeker;
        }
        
        private static async Task<IClicker> CreateClicker(ChromeOptionsProvider optionsProvider)
        {
            var clicker = new SeleniumClicker(optionsProvider.Get());
            
            clicker.GotoHome();
            await Task.Delay(1000);
            clicker.CloseCookiesInfo();
            
            return clicker;
        }
        
        private static async Task ExploringStep(BookInfoSeeker seeker, IBooksRepository repository)
        {
            await Task.Delay(GetRandomDelay());
            
            var randomBook = await repository.GetRandomNotFilledBookAsync();
            
            await Task.Delay(GetRandomDelay());
            
            var bookExists = seeker.ReadBookInfo(randomBook);
            
            if (!bookExists)
            {
                BookInfo.MarkAsNotFound(randomBook);
            }
            else
            {
                if (randomBook?.BookType == null)
                {
                    throw new InvalidOperationException(
                        "something went wrong, book properties was not read");
                }
                
                if (randomBook.ClosureDate.Length <= 4)
                {
                    var properties = seeker.ReadProperties();
                    randomBook.AddNewProperties(properties);
                }
            }

            await repository.UpdateBookAsync(randomBook);
            await repository.AddPropertyFromBookAsync(randomBook);
                    
            await Task.Delay(GetRandomDelay());

            seeker.BackToCriteria();
        }
        
        private static int GetRandomDelay() 
            => Random.Next(800, 2000);
    }
}