using System;
using System.Globalization;
using System.Text;

namespace EkwClicker
{
    internal class ProgramInput
    {
        public string DatabaseFile { get; set; }
        public string CourtCode { get; set; }
        public int? NumberFrom { get; set; }
        public int? NumberTo { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder()
                .AppendLine($"Database: {DatabaseFile}")
                .AppendLine($"Court code: {CourtCode}");
            
            if (NumberFrom.HasValue && NumberTo.HasValue)
            {
                sb.Append("Find codes ")
                    .Append($"from {CourtCode}/{NumberFrom.Value.ToString("D8", CultureInfo.InvariantCulture)}")
                    .AppendLine($" to {CourtCode}/{NumberFrom.Value.ToString("D8", CultureInfo.InvariantCulture)}");
            }
            else
            {
                sb.AppendLine("Find codes defined in existing database");
            }
            
            return sb.ToString();
        }

        public void ThrowIfInvalid()
        {
            if (string.IsNullOrEmpty(DatabaseFile))
            {
                throw new ArgumentException(
                    "is required", nameof(DatabaseFile));
            }
            
            if (string.IsNullOrEmpty(CourtCode))
            {
                throw new ArgumentException(
                    "is required", nameof(CourtCode));
            }
            
            if (CourtCode.Length != 4 && CourtCode != "x")
            {
                throw new ArgumentException(
                    "must contains 4 characters", nameof(CourtCode));
            }
            
            if (NumberFrom <= 0 || NumberFrom >= NumberTo)
            {
                throw new ArgumentException(
                    $"{nameof(NumberFrom)} must be positive and less than {nameof(NumberTo)}");
            }
        }
    }
}