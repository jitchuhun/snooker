using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Snooker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            // 2. feladat
            List<Player> data = new List<Player>();
            List<string> errorLines = new List<string>();

            using (StreamReader reader = new StreamReader("snooker.txt", Encoding.GetEncoding("ISO-8859-2")))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] fields = line.Split(';');

                    int rank;
                    if (!int.TryParse(fields[0], out rank))
                    {
                        errorLines.Add(line);
                        continue;
                    }

                    string name = fields[1];
                    string nationality = fields[2];

                    decimal earnings;
                    if (!decimal.TryParse(fields[3], out earnings))
                    {
                        errorLines.Add(line);
                        continue;
                    }

                    Player player = new Player(rank, name, nationality, earnings);
                    data.Add(player);
                }
            }

            // 3. feladat
            Console.WriteLine($"3. feladat: A világranglistán {data.Count} versenyző szerepel");

            // 4. feladat
            var averageEarnings = data.Average(p => p.Earnings);
            Console.WriteLine($"4. feladat: A versenyzők átlagosan {averageEarnings:F2} fontot kerestek");

            // 5. feladat
            var chinaPlayer = data.Where(p => p.Country == "Kína").OrderByDescending(p => p.Earnings).FirstOrDefault();
            if (chinaPlayer == null)
            {
                Console.WriteLine("5. feladat: A legjobban kereső kínai versenyző:\n    Nincs kínai versenyző az adatok között");
            }
            else
            {
                var exchangeRate = 380m;
                var earningsInForint = chinaPlayer.Earnings * exchangeRate;
                Console.WriteLine($"5. feladat: A legjobban kereső kínai versenyző:\n" +
                    $"    Helyezés: {chinaPlayer.Rank}\n" +
                    $"    Név: {chinaPlayer.Name}\n" +
                    $"    Ország: {chinaPlayer.Country}\n" +
                    $"    Nyeremény összege: {earningsInForint:N2} Ft");
            }

            // 6. feladat
            var isNorwegianPlayerOnList = data.Any(p => p.Country == "Norvégia");
            Console.WriteLine($"6. feladat: A versenyzők között {(isNorwegianPlayerOnList ? "van" : "nincs")} norvég versenyző.");

            // 7. feladat
            var countryGroups = data.GroupBy(p => p.Country).Where(g => g.Count() > 4).OrderByDescending(g => g.Count());
            Console.WriteLine("7. feladat: Statisztika");
            foreach (var group in countryGroups)
            {
                Console.WriteLine($"    {group.Key} - {group.Count()} fő");
            }

            Console.ReadLine();
        }
    }

    class Player
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal Earnings { get; set; }

        public Player(int rank, string name, string country, decimal earnings)
        {
            Rank = rank;
            Name = name;
            Country = country;
            Earnings = earnings;
        }
    }
}
