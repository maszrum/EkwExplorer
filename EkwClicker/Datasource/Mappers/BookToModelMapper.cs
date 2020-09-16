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
                bookEntity.CourtCode, bookEntity.Number, bookEntity.ControlDigit);

            _book = new BookInfo(bookEntity.Id, bookNumber)
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
            var propertyNumber = new PropertyNumber(
                propertyNumberEntity.Id, propertyNumberEntity.Number);

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