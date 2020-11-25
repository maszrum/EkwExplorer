using System;
using EkwClicker.Datasource.Entities;
using EkwClicker.Models;

namespace EkwClicker.Datasource.Mappers
{
    internal class BookToModelMapper
    {
        private BookInfo _book;

        public BookToModelMapper Map(BookEntity bookEntity)
        {
            var bookNumber = new BookNumber(
                bookEntity.CourtCode.ToUpper(), bookEntity.Number, bookEntity.ControlDigit);

            var bookId = Guid.ParseExact(bookEntity.Id, "N");
            
            _book = new BookInfo(bookId, bookNumber)
            {
                BookType = bookEntity.BookType,
                ClosureDate = bookEntity.ClosureDate,
                Location = bookEntity.Location,
                OpeningDate = bookEntity.OpeningDate,
                Owner = bookEntity.Owner
            };

            return this;
        }

        public BookToModelMapper WithProperty(PropertyNumberEntity propertyNumberEntity)
        {
            var propertyId = Guid.ParseExact(propertyNumberEntity.Id, "n");
            
            var propertyNumber = new PropertyNumber(
                propertyId, propertyNumberEntity.Number);

            _book.PropertyNumbers.Add(propertyNumber);

            return this;
        }

        public BookInfo Finish()
        {
            return _book ??
                   throw new ArgumentNullException($"{nameof(Map)} method was not called");
        }
    }
}