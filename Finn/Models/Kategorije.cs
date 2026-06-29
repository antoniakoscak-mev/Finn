namespace Finn.Models
{
    public class Kategorije
    {
        public static List<string> OsobniPrihodi =>
            new()
            {
                "Plaća",
                "Bonus",
                "Investicije",
                "Poklon",
                "Ostalo"
            };

        public static List<string> PoslovniPrihodi =>
            new()
            {
                "Prodaja",
                "Usluge",
                "Najam",
                "Ostalo"
            };

        public static List<string> OsobniRashodi =>
            new()
            {
                "Hrana",
                "Stanovanje",
                "Prijevoz",
                "Shopping",
                "Zdravlje",
                "Putovanja",
                "Ostalo"
            };

        public static List<string> PoslovniRashodi =>
            new()
            {
                "Marketing",
                "Softver",
                "Plaće",
                "Porezi",
                "Oprema",
                "Najam prostora",
                "Ostalo"
            };
    }
}

