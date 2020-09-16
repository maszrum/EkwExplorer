using System;
using System.Linq;

namespace EkwClicker.Models
{
    internal class BookNumber
    {
        public BookNumber(string courtCode, string number)
        {
            if (courtCode.Length != 4)
            {
                throw new ArgumentException(
                    "must have exactly 4 characters", nameof(courtCode));
            }
            if (number.Length != 8 || number.Any(c => !char.IsDigit(c)))
            {
                throw new ArgumentException(
                    "must have 8 digits", nameof(number));
            }
            
            CourtCode = courtCode;
            Number = number;
        }
        
        public BookNumber(string courtCode, string number, int controlDigit) 
            : this(courtCode, number)
        {
            ControlDigit = controlDigit;
        }

        public Guid Id { get; set; }
        public string CourtCode { get; }
        public string Number { get; }
        public int? ControlDigit { get; private set; }
        public int Length => CourtCode.Length + Number.Length;
        
        public void SetControlDigit(int value)
        {
            ControlDigit = value;
        }

        public override string ToString()
        {
            return ControlDigit.HasValue
                ? $"{CourtCode}/{Number}/{ControlDigit}"
                : $"{CourtCode}/{Number}/?";
        }

        public static BookNumber Parse(string input)
        {
            var parts = input.Split('/');

            if (parts.Length < 2)
            {
                throw new ArgumentException(
                    "invalid format", nameof(input));
            }

            return new BookNumber(parts[0], parts[1]);
        }
    }
}