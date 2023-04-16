using EkwExplorer.Core;
using Serilog;

namespace EkwExplorer.FakeScraper;

public class FakeExplorer : IBooksExplorer
{
    public FakeExplorer(ILogger logger, IBooksRepository booksRepository)
    {
        _logger = logger;
        _booksRepository = booksRepository;
    }

    private readonly ILogger _logger;
    private readonly IBooksRepository _booksRepository;

    private readonly Random _random = new Random(DateTime.Now.Millisecond);

    private readonly FakeDataGenerator _dataGenerator = new FakeDataGenerator();

    public async Task Explore(CancellationToken cancellationToken)
    {
        var downloadedBooks = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var randomBook = await _booksRepository.GetRandomNotFilledBookAsync();

            _dataGenerator.WriteDataToBook(randomBook);

            await _booksRepository.UpdateBookAsync(randomBook);
            await _booksRepository.AddPropertyFromBookAsync(randomBook);

            downloadedBooks++;
            _logger.Debug("Downloaded books: {DownloadedBooks}", downloadedBooks);

            await RandomDelay(cancellationToken);
        }

        cancellationToken.ThrowIfCancellationRequested();
    }

    private async Task RandomDelay(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(GetRandomDelay(), cancellationToken);
        }
        catch (TaskCanceledException tce)
        {
            throw new OperationCanceledException(tce.Message, tce);
        }
    }

    private int GetRandomDelay()
        => _random.Next(800, 2000);
}
