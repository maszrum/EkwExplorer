using System.Text;

namespace EkwClicker
{
    internal class BookInfo
    {
        public string BookType { get; set; }
        public string OpeningDate { get; set; }
        public string ClosureDate { get; set; }
        public string Location { get; set; }
        public string Owner { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendLine($"Type: {BookType}")
                .AppendLine($"Opened: {OpeningDate}")
                .AppendLine($"Closed: {ClosureDate}")
                .AppendLine($"Location: {Location}")
                .AppendLine($"Owner: {Owner}")
                .ToString();
        }
    }
}