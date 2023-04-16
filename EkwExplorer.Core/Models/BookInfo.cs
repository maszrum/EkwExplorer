using System.Text;

namespace EkwExplorer.Core.Models;

public class BookInfo
{
    public BookInfo(Guid id, BookNumber number)
    {
        if (!number.ControlDigit.HasValue)
            throw new ArgumentException(
                "must be filled", nameof(number.ControlDigit));

        Number = number;
        Id = id;
    }

    public Guid Id { get; }
    public string BookType { get; set; }
    public string OpeningDate { get; set; }
    public string ClosureDate { get; set; }
    public string Location { get; set; }
    public string Owner { get; set; }
    public List<PropertyNumber> PropertyNumbers { get; } = new List<PropertyNumber>();
    public BookNumber Number { get; }

    public void AddNewProperties(IEnumerable<string> propertyNumbers)
    {
        var properties = propertyNumbers
            .Select(p => new PropertyNumber(Guid.NewGuid(), p));

        PropertyNumbers.AddRange(properties);
    }

    public override string ToString()
    {
        var properties = PropertyNumbers.Count > 0
            ? string.Join(", ", PropertyNumbers)
            : "no properties";

        return new StringBuilder()
            .AppendLine($"Type: {BookType}")
            .AppendLine($"Opened: {OpeningDate}")
            .AppendLine($"Closed: {ClosureDate}")
            .AppendLine($"Location: {Location}")
            .AppendLine($"Owner: {Owner}")
            .AppendLine($"Properties: {properties}")
            .ToString();
    }

    public static void MarkAsNotFound(BookInfo bookInfo)
    {
        bookInfo.BookType = "not found";
        bookInfo.OpeningDate = "-";
        bookInfo.ClosureDate = "-";
        bookInfo.Location = "-";
        bookInfo.Owner = "-";
    }
}
