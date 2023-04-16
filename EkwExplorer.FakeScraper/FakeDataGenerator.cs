using EkwExplorer.Core.Models;

namespace EkwExplorer.FakeScraper
{
    internal class FakeDataGenerator
    {
        private const int MinProperties = 0;
        private const int MaxProperties = 10;
        private const int MinDaysBack = 5;
        private const int MaxDaysBack = 10 * 365;
        private const int MinPropertyNumber = 1;
        private const int MaxPropertyNumber = 300;
        private const int MinPropertySubnumber = 1;
        private const int MaxPropertySubnumber = 10;
        private const int ClosureDateProbability = 10;

        private readonly WeightedRandomizer<string> _bookTypes = new WeightedRandomizer<string>()
        {
            ["Nieruchomość gruntowa"] = 20,
            ["Grunt oddany w użytkowanie wieczyste"] = 1,
            ["Grunt oddany w użytkowanie wieczyste i budynek stanowiący odrębną nieruchomość"] = 1,
            ["Grunt oddany w użytkowanie wieczyste i urządzenie stanowiące odrębną nieruchomość"] = 1,
            ["Grunt oddany w użytkowanie wieczyste, budynek i urządzenie stanowiące odrębną nieruchomość"] = 1,
            ["Budynek stanowiący odrębną nieruchomość"] = 2,
            ["Lokal stanowiący odrębną nieruchomość"] = 4,
            ["Spółdzielcze własnościowe prawo do lokalu"] = 2
        };

        private readonly Randomizer<string> _locations = new Randomizer<string>()
        {
            "Głubczyce",
            "Zgierz",
            "Rymanów",
            "Sokółka",
            "Zakroczym",
            "Zduńska Wola",
            "Trzciel",
            "Czempiń",
            "Józefów",
            "Aleksandrów Łódzki",
            "Karlino",
            "Złotoryja",
            "Brańsk",
            "Małomice",
            "Mrocza",
            "Łaskarzew",
            "Gołańcz",
            "Witkowo",
            "Nowa Ruda",
            "Leżajsk",
            "Zawadzkie",
            "Międzyrzec Podlaski",
            "Krynki",
            "Międzyrzecz",
            "Kościan",
            "Pasłęk",
            "Radków",
            "Bierutów",
            "Kraśnik",
            "Leśnica"
        };

        private readonly RandomNamesGenerator _namesGenerator = new RandomNamesGenerator();
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public void WriteDataToBook(BookInfo bookInfo)
        {
            static string FormatDateTime(DateTime dateTime) =>
                dateTime.ToString("yyyy-MM-dd");

            bookInfo.BookType = _bookTypes.Next().ToUpperInvariant();
            bookInfo.OpeningDate = FormatDateTime(GetRandomDate());
            bookInfo.Location = _locations.Next().ToUpperInvariant();
            bookInfo.Owner = _namesGenerator.Generate(NameFormat.SurnameName).ToUpperInvariant();

            var hasClosureDate = _random.Next(1, 101) <= ClosureDateProbability;
            bookInfo.ClosureDate = hasClosureDate ? FormatDateTime(GetRandomDate()) : "---";

            var propertiesCount = _random.Next(MinProperties, MaxProperties + 1);
            var properties = Enumerable
                .Repeat(string.Empty, propertiesCount)
                .Select(_ => GetRandomPropertyNumber());

            bookInfo.AddNewProperties(properties);
        }

        private DateTime GetRandomDate()
        {
            var minusDays = _random.Next(MinDaysBack, MaxDaysBack + 1);
            return DateTime.UtcNow.AddDays(-minusDays);
        }

        private string GetRandomPropertyNumber()
        {
            var hasSubnumber = _random.Next() % 2 == 0;
            var propertyNumber = _random.Next(MinPropertyNumber, MaxPropertyNumber + 1);
            return hasSubnumber
                ? $"{propertyNumber}/{_random.Next(MinPropertySubnumber, MaxPropertySubnumber + 1)}"
                : propertyNumber.ToString();
        }
    }
}
