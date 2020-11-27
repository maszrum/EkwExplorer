using System;
using System.Collections.Generic;
using EkwExplorer.Core;
using EkwExplorer.Core.Models;

namespace EkwExplorer.ChromeScraper
{
    internal class BookInfoSeeker
    {
        public BookInfoSeeker(IClicker clicker)
        {
            Clicker = clicker;
        }
        
        public IClicker Clicker { get; }
        
        public bool ReadBookInfo(BookInfo bookInfo)
        {
            if (!bookInfo.Number.ControlDigit.HasValue)
            {
                throw new ArgumentException(
                    "must be filled with value", nameof(bookInfo.Number.ControlDigit));
            }
            
            var bookNumber = bookInfo.Number;
            
            Clicker.FillTextbox("kodWydzialuInput", bookNumber.CourtCode);
            Clicker.FillTextbox("numerKsiegiWieczystej", bookNumber.Number);
            Clicker.FillTextbox("cyfraKontrolna", bookNumber.ControlDigit.ToString());
             
            Clicker.ClickButtonById("wyszukaj");

            if (Clicker.CheckIfAnyError()) throw new Exception("captcha error");

            if (!Clicker.CheckIfNotFound())
            {
                bookInfo.BookType = Clicker.GetValueFromTable("Typ księgi wieczystej");
                bookInfo.OpeningDate = Clicker.GetValueFromTable("Data zapisania księgi wieczystej");
                bookInfo.ClosureDate = Clicker.GetValueFromTable("Data zamknięcia księgi wieczystej");
                bookInfo.Location = Clicker.GetValueFromTable("Położenie");
                bookInfo.Owner = Clicker.GetValueFromTable("Właściciel");

                return true;
            }

            return false;
        }

        public IEnumerable<string> ReadProperties()
        {
            Clicker.ClickButtonById("przyciskWydrukZwykly");

            var numbers = Clicker.GetPropertyNumbers();

            Clicker.ClickButtonByName("Wykaz");

            return numbers;
        }

        public void BackToCriteria()
        {
            Clicker.ClickButtonById("powrotDoKryterii");
        }
    }
}