﻿using System;
using System.Threading.Tasks;

namespace EkwClicker
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
            _clicker.FillTextbox("kodWydzialuInput", "NS1T");
            _clicker.FillTextbox("numerKsiegiWieczystej", "00046573");
            _clicker.FillTextbox("cyfraKontrolna", "5");
             
            _clicker.ClickButton("wyszukaj");

            if (_clicker.CheckIfAnyError()) throw new Exception("captcha error");

            var bookType = _clicker.GetValueFromTable("Typ księgi wieczystej");
            var openingDate = _clicker.GetValueFromTable("Data zapisania księgi wieczystej");
            var closureDate = _clicker.GetValueFromTable("Data zamknięcia księgi wieczystej");
            var location = _clicker.GetValueFromTable("Położenie");
            var owner = _clicker.GetValueFromTable("Właściciel");

            _clicker.ClickButton("powrotDoKryterii");
            
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