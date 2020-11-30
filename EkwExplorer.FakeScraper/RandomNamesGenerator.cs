using System;

namespace EkwExplorer.FakeScraper
{
    public enum NameFormat
    {
        NameSurname,
        SurnameName
    }

    internal class RandomNamesGenerator
    {
        private readonly Randomizer<string> _femaleNames = new Randomizer<string>()
        {
            "Anna",
            "Maria",
            "Katarzyna",
            "Małgorzata",
            "Agnieszka",
            "Barbara",
            "Krystyna",
            "Ewa",
            "Elżbieta",
            "Zofia",
            "Teresa",
            "Magdalena",
            "Joanna",
            "Janina",
            "Monika",
            "Danuta",
            "Jadwiga",
            "Aleksandra",
            "Halina",
            "Irena",
            "Beata",
            "Marta",
            "Renata",
            "Alicja",
            "Urszula",
            "Paulina",
            "Justyna",
            "Stanisława",
            "Bożena",
            "Natalia",
            "Marianna",
            "Dorota",
            "Helena",
            "Karolina",
            "Grażyna",
            "Jolanta",
            "Iwona"
        };

        private readonly Randomizer<string> _maleNames = new Randomizer<string>()
        {
            "Piotr",
            "Krzysztof",
            "Andrzej",
            "Jan",
            "Stanisław",
            "Tomasz",
            "Paweł",
            "Marcin",
            "Michał",
            "Marek",
            "Grzegorz",
            "Józef",
            "łukasz",
            "Adam",
            "Zbigniew",
            "Jerzy",
            "Tadeusz",
            "Mateusz",
            "Dariusz",
            "Mariusz",
            "Wojciech",
            "Ryszard",
            "Jakub",
            "Henryk",
            "Robert",
            "Rafał",
            "Kazimierz",
            "Jacek",
            "Maciej",
            "Kamil",
            "Janusz",
            "Marian",
            "Mirosław",
            "Jarosław",
            "Sławomir",
            "Dawid",
            "Wiesław"
        };

        private readonly Randomizer<string> _surnames = new Randomizer<string>()
        {
            "Nowak",
            "Kowalski",
            "Wiśniewski",
            "Wójcik",
            "Kowalczyk",
            "Kamiński",
            "Lewandowski",
            "Zieliński",
            "Szymański",
            "Woźniak",
            "Dąbrowski",
            "Kozłowski",
            "Jankowski",
            "Mazur",
            "Wojciechowski",
            "Kwiatkowski",
            "Krawczyk",
            "Kaczmarek",
            "Piotrowski",
            "Grabowski",
            "Zając",
            "Pawłowski",
            "Michalski",
            "Król",
            "Wieczorek",
            "Jabłoński",
            "Wróbel",
            "Nowakowski",
            "Majewski",
            "Olszewski"
        };

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public string GenerateMale(NameFormat nameFormat = NameFormat.NameSurname)
        {
            var name = _maleNames.Next();
            var surname = _surnames.Next();

            return nameFormat switch
            {
                NameFormat.NameSurname => $"{name} {surname}",
                NameFormat.SurnameName => $"{surname} {name}",
                _ => throw new ArgumentOutOfRangeException(nameof(nameFormat))
            };
        }

        public string GenerateFemale(NameFormat nameFormat = NameFormat.NameSurname)
        {
            var name = _femaleNames.Next();
            var surname = MaleSurnameToFemale(_surnames.Next());

            return nameFormat switch
            {
                NameFormat.NameSurname => $"{name} {surname}",
                NameFormat.SurnameName => $"{surname} {name}",
                _ => throw new ArgumentOutOfRangeException(nameof(nameFormat))
            };
        }

        public string Generate(NameFormat nameFormat = NameFormat.NameSurname)
        {
            var isFemale = _random.Next() % 2 == 0;

            return isFemale 
                ? GenerateFemale(nameFormat) 
                : GenerateMale(nameFormat);
        }

        private static string MaleSurnameToFemale(string surname) =>
            surname.Replace("ski", "ska");
    }
}
