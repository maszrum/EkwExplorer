using EkwExplorer.Core.Models;

namespace EkwExplorer.Core.Algorithms;

public class DatasourceSeeder : IDataSeeder
{
    private readonly IBooksRepository _repository;

    public DatasourceSeeder(IBooksRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task SeedAsync(BookNumber fromNumber, BookNumber toNumber)
    {
        if (fromNumber.CourtCode != toNumber.CourtCode)
        {
            throw new ArgumentException(
                $"{nameof(fromNumber.CourtCode)} must be same as {nameof(toNumber.CourtCode)}");
        }

        var from = fromNumber.NumberAsInt;
        var to = toNumber.NumberAsInt;

        if (from >= to)
        {
            throw new ArgumentException(
                $"{nameof(fromNumber.Number)} must be less than {nameof(toNumber.Number)}");
        }

        return SeedDatabase(fromNumber.CourtCode, from, to);
    }

    private async Task SeedDatabase(string court, int from, int to)
    {
        var decoder = new ControlDigitDecoder();

        for (var i = from; i <= to; i++)
        {
            var number = i.ToString("D8");
            var bookNumber = new BookNumber(court, number);
            var controlDigit = decoder.Decode(bookNumber);
            bookNumber.SetControlDigit(controlDigit);

            var bookInfo = new BookInfo(Guid.NewGuid(), bookNumber);

            await _repository.AddBookAsync(bookInfo);
        }
    }
}
