using System;

namespace EkwExplorer.ConsoleApp
{
    internal class ProgramInputConsoleReader
    {
        private readonly ProgramInput _input;

        public ProgramInputConsoleReader(ProgramInput programInput)
        {
            _input = programInput;
        }

        public void Read()
        {
            Console.WriteLine("Enter database file:");
            _input.DatabaseFile = Console.ReadLine()?.Trim();
            
            Console.WriteLine("Enter 4-character court code or 'x' to continue exploring:");
            _input.CourtCode = Console.ReadLine()?.Trim();
            
            if (_input.CourtCode != "x")
            {
                Console.WriteLine("Enter starting number:");
                var numberFromInput = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(numberFromInput) || !int.TryParse(numberFromInput, out var numberFrom))
                {
                    throw new ArgumentException(
                        "must be a number", nameof(_input.NumberFrom));
                }
                _input.NumberFrom = numberFrom;
                
                Console.WriteLine("Enter finish number:");
                var numberToInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(numberToInput) || !int.TryParse(numberToInput, out var numberTo))
                {
                    throw new ArgumentException(
                        "must be a number", nameof(_input.NumberFrom));
                }
                _input.NumberTo = numberTo;
            }

            Console.WriteLine("Use fake data? (y/n)");
            var useFake = Console.ReadLine()?.Trim().ToLower();
            _input.FakeData = useFake == "true" || useFake == "1" || useFake == "yes" || useFake == "y";
            
            _input.ThrowIfInvalid();
        }
    }
}