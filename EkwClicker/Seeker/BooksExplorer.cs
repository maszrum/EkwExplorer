using System;
using System.Threading.Tasks;
using EkwClicker.Core;
using EkwClicker.Models;
using Serilog;

namespace EkwClicker.Seeker
{
    internal class BooksExplorer
    {
        private static readonly ChromeOptionsProvider ChromeOptions = new ChromeOptionsProvider();
        
        private readonly ILogger _logger;
        private readonly IBooksRepository _booksRepository;
        
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        
        private BookInfoSeeker _seeker;
        
        public BooksExplorer(ILogger logger, IBooksRepository booksRepository)
        {
            _logger = logger;
            _booksRepository = booksRepository;
        }
        
        public async Task Open()
        {
            _seeker = await OpenSeeker();
        }
        
        public async Task Explore()
        {
            var captchaErrors = 0;
            var downloadedBooks = 0;
            
            while (true)
            {
                try
                {
                    await ExploringStep();
                    downloadedBooks++;
                    captchaErrors = 0;
                }
                catch (Exception exception) when (exception.Message.Contains("captcha"))
                {
                    _logger.Warning("Detected captcha.");
                    
                    captchaErrors++;
                    if (captchaErrors > 5)
                    {
                        captchaErrors = 0;
                        
                        _logger.Information("Reopening chrome driver...");
                        _seeker = await ReopenSeeker(_seeker);
                        _logger.Information("Opened new istance of chrome driver");
                    }
                }
                catch (Exception ex) when (ex.GetType().Namespace == "OpenQA.Selenium")
                {
                    _logger.Error(ex, "Error in Selenium driver");
                    _seeker = await ReopenSeeker(_seeker);
                }
                catch (InvalidOperationException ioe) when (ioe.Message.Contains("was not read"))
                {
                    _logger.Error(ioe, "Error while exploring ekw");
                    _seeker = await ReopenSeeker(_seeker);
                }
                
                _logger.Debug("Downloaded books: {DownloadedBooks}", downloadedBooks);
            }
        }
        
        private async Task ExploringStep()
        {
            await Task.Delay(GetRandomDelay());
            
            var randomBook = await _booksRepository.GetRandomNotFilledBookAsync();
            
            await Task.Delay(GetRandomDelay());
            
            var bookExists = _seeker.ReadBookInfo(randomBook);
            
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
                    var properties = _seeker.ReadProperties();
                    randomBook.AddNewProperties(properties);
                }
            }

            await _booksRepository.UpdateBookAsync(randomBook);
            await _booksRepository.AddPropertyFromBookAsync(randomBook);
                    
            await Task.Delay(GetRandomDelay());

            _seeker.BackToCriteria();
        }
        
        private int GetRandomDelay() 
            => _random.Next(800, 2000);
        
        private static Task<BookInfoSeeker> OpenSeeker()
            => ReopenSeeker(null);
        
        private static async Task<BookInfoSeeker> ReopenSeeker(BookInfoSeeker seeker)
        {
            seeker?.Clicker.Dispose();
            
            var clicker = await CreateClicker(ChromeOptions);
            
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
    }
}