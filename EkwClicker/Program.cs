using System;
using System.Threading.Tasks;

namespace EkwClicker
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			// wyszukaj

			var url = "https://przegladarka-ekw.ms.gov.pl/eukw_prz/KsiegiWieczyste/wyszukiwanieKW";
			using var clicker = new SeleniumClicker(url);

			clicker.GotoHome();
			await Task.Delay(1000);
			clicker.CloseCookiesInfo();

			clicker.FillTextbox("kodWydzialuInput", "NS1T");
			clicker.FillTextbox("numerKsiegiWieczystej", "00046573");
			clicker.FillTextbox("cyfraKontrolna", "5");

			clicker.ClickButtonById("wyszukaj");

			if (clicker.CheckIfAnyError())
			{
				throw new Exception("captcha error");
			}

			string bookType = clicker.GetValueFromTable("Typ księgi wieczystej");
			string openingDate = clicker.GetValueFromTable("Data zapisania księgi wieczystej");
			string closureDate = clicker.GetValueFromTable("Data zamknięcia księgi wieczystej");
			string location = clicker.GetValueFromTable("Położenie");
			string owner = clicker.GetValueFromTable("Właściciel");

			clicker.ClickButtonById("przyciskWydrukZwykly");
			var numbers = clicker.GetPropertyNumbers();

			clicker.ClickButtonByName("Wykaz");
			clicker.ClickButtonById("powrotDoKryterii");
		}
	}
}
