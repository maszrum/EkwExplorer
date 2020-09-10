using System;
using System.Linq;

namespace EkwClicker
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
        
        public BookNumber(string courtCode, string number, int controlNumber) 
            : this(courtCode, number)
        {
            ControlNumber = controlNumber;
        }
        
        public string CourtCode { get; }
        public string Number { get; }
        public int? ControlNumber { get; private set; }
        
        public void SetControlNumber(int value)
        {
            ControlNumber = value;
        }
    }
}