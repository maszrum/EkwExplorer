using System;
using System.Collections.Generic;
using System.Linq;
using EkwClicker.Datasource.Entities;
using EkwClicker.Models;

namespace EkwClicker.Datasource.Mappers
{
    internal class BookToEntityMapper
    {
        private readonly BookInfo _book;

        public BookToEntityMapper(BookInfo book)
        {
            _book = book ?? throw new ArgumentNullException(nameof(book));
        }

        public BookEntity MapBook()
        {
            if (!_book.Number.ControlDigit.HasValue)
                throw new ArgumentException(
                    "must be filled with value", nameof(_book.Number.ControlDigit));

            var filled = !string.IsNullOrWhiteSpace(_book.BookType);

            var entity = new BookEntity
            {
                BookType = _book.BookType,
                ClosureDate = _book.ClosureDate,
                ControlDigit = _book.Number.ControlDigit.Value,
                CourtCode = _book.Number.CourtCode.ToUpper(),
                Filled = filled,
                Id = _book.Id.ToString("N"),
                Location = _book.Location,
                Number = _book.Number.Number,
                OpeningDate = _book.OpeningDate,
                Owner = _book.Owner
            };

            return entity;
        }

        public IEnumerable<PropertyNumberEntity> MapPropertyNumbers()
        {
            return _book.PropertyNumbers.Select(property =>
                new PropertyNumberEntity
                {
                    BookId = _book.Id,
                    Id = property.Id.ToString("N"),
                    Number = property.Number
                });
        }
    }
}