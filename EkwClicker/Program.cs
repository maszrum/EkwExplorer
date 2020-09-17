using System;
using System.Threading.Tasks;
using EkwClicker.Algorithms;
using EkwClicker.Datasource;
using EkwClicker.Datasource.Repositories;
using EkwClicker.Models;
using EkwClicker.Seeker;

// ReSharper disable ClassNeverInstantiated.Global

namespace EkwClicker
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var input = ProgramInput.ReadFromConsole();
            
            var connection = DbAccess.Exists(input.DatabaseFile)
                ? await DbAccess.Connect(input.DatabaseFile)
                : await DbAccess.Create(input.DatabaseFile);

            await using (connection)
            {
                var repository = new BooksRepository(connection);
                var seeder = new DatasourceSeeder(repository);

                if (input.NumberFrom.HasValue && input.NumberTo.HasValue)
                {
                    var numberFrom = new BookNumber(input.CourtCode, input.NumberFrom.Value.ToString("D8"));
                    var numberTo = new BookNumber(input.CourtCode, input.NumberTo.Value.ToString("D8"));
                    await seeder.SeedAsync(numberFrom, numberTo);
                }
                
                var randomBook = await repository.GetRandomNotFilledBookAsync();
            }

            DbAccess.Remove(input.DatabaseFile);
            
            var url = "https://przegladarka-ekw.ms.gov.pl/eukw_prz/KsiegiWieczyste/wyszukiwanieKW";
            using var clicker = new SeleniumClicker(url);

            clicker.GotoHome();
            await Task.Delay(1000);
            clicker.CloseCookiesInfo();

            clicker.FillTextbox("kodWydzialuInput", "NS1T");
            clicker.FillTextbox("numerKsiegiWieczystej", "00046573");
            clicker.FillTextbox("cyfraKontrolna", "5");

            clicker.ClickButtonById("wyszukaj");

            if (clicker.CheckIfAnyError()) throw new Exception("captcha error");

            var bookType = clicker.GetValueFromTable("Typ księgi wieczystej");
            var openingDate = clicker.GetValueFromTable("Data zapisania księgi wieczystej");
            var closureDate = clicker.GetValueFromTable("Data zamknięcia księgi wieczystej");
            var location = clicker.GetValueFromTable("Położenie");
            var owner = clicker.GetValueFromTable("Właściciel");

            clicker.ClickButtonById("przyciskWydrukZwykly");
            var numbers = clicker.GetPropertyNumbers();

            clicker.ClickButtonByName("Wykaz");
            clicker.ClickButtonById("powrotDoKryterii");
        }
    }
}