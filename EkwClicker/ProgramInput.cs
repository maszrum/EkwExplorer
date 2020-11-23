using System;
using System.Collections.Generic;
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

        public static ProgramInput ReadFromConsole()
        {
            var input = new ProgramInput();
            
            Console.WriteLine("Enter database file:");
            input.DatabaseFile = Console.ReadLine()?.Trim();
            
            Console.WriteLine("Enter 4-character court code or 'x' to continue exploring:");
            input.CourtCode = Console.ReadLine()?.Trim();
            
            if (input.CourtCode != "x")
            {
                Console.WriteLine("Enter starting number:");
                var numberFromInput = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(numberFromInput) || !int.TryParse(numberFromInput, out var numberFrom))
                {
                    throw new ArgumentException(
                        "must be a number", nameof(input.NumberFrom));
                }
                input.NumberFrom = numberFrom;
                
                Console.WriteLine("Enter finish number:");
                var numberToInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(numberToInput) || !int.TryParse(numberToInput, out var numberTo))
                {
                    throw new ArgumentException(
                        "must be a number", nameof(input.NumberFrom));
                }
                input.NumberTo = numberTo;
            }
            
            input.ThrowIfInvalid();
            
            return input;
        }
        
        public static ProgramInput ReadFromJsonFile(string configFile)
        {
            throw new NotImplementedException();
        }
        
        public static ProgramInput ReadFromArgs(IReadOnlyList<string> args)
        {
            throw new NotImplementedException();
        }
        
        private void ThrowIfInvalid()
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