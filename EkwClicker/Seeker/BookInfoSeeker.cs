using System;
using EkwClicker.Core;
using EkwClicker.Models;

namespace EkwClicker.Seeker
{
    internal class BookInfoSeeker
    {
        private readonly IClicker _clicker;

        public BookInfoSeeker(IClicker clicker)
        {
            _clicker = clicker;
        }
        
        public BookInfo ReadBookInfo(BookNumber bookNumber)
        {
            _clicker.FillTextbox("kodWydzialuInput", bookNumber.CourtCode);
            _clicker.FillTextbox("numerKsiegiWieczystej", bookNumber.Number);
            _clicker.FillTextbox("cyfraKontrolna", bookNumber.ControlDigit.ToString());
             
            _clicker.ClickButtonById("wyszukaj");

            if (_clicker.CheckIfAnyError()) throw new Exception("captcha error");

            var bookType = _clicker.GetValueFromTable("Typ księgi wieczystej");
            var openingDate = _clicker.GetValueFromTable("Data zapisania księgi wieczystej");
            var closureDate = _clicker.GetValueFromTable("Data zamknięcia księgi wieczystej");
            var location = _clicker.GetValueFromTable("Położenie");
            var owner = _clicker.GetValueFromTable("Właściciel");

            _clicker.ClickButtonById("powrotDoKryterii");
            
            return new BookInfo()
            {
                BookType = bookType,
                OpeningDate = openingDate,
                ClosureDate = closureDate,
                Location = location,
                Owner = owner
            };
        }
    }
}