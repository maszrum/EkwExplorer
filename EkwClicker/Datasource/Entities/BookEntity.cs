using System;

namespace EkwClicker.Datasource.Entities
{
    internal class BookEntity
    {
        public Guid Id { get; set; }
        
        public string CourtCode { get; set; }
        public string Number { get; set; }
        public int ControlDigit { get; set; }
        
        public string BookType { get; set; }
        public string OpeningDate { get; set; }
        public string ClosureDate { get; set; }
        public string Location { get; set; }
        public string Owner { get; set; }
        public bool Filled { get; set; }
    }
}