using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRollerCLI
{
    public static class DiceRoller
    {
        public class DiceRoll
        {
            public int Dice;
            public int Roll;
        }

        public static void RollDice()
        {
            var rand = new Random();
            var text = string.Empty;
            var allDice = new List<List<DiceRoll>>();
            do
            {
                Console.Write("> ");
                var tempText = Console.ReadLine().Trim().ToLower();
                if (tempText == "h" || tempText == "help")
                {
                    Console.WriteLine("- [n]d[m][ + [x]d[y]] - dice roll, optional plus for multiple dice");
                    Console.WriteLine("- c, clear - clear stats");
                    Console.WriteLine("- r, rand - reset random number generator");
                    Console.WriteLine("- q - quit");
                    text = string.Empty;
                }
                else if (tempText == "c" || tempText == "clear")
                {
                    Console.WriteLine("cleared stats");
                    allDice.Clear();
                    text = string.Empty;
                }
                else if (tempText == "r" || tempText == "rand")
                {
                    Console.WriteLine("rand reset");
                    rand = new Random();
                    text = string.Empty;
                }
                else if (tempText != string.Empty)
                {
                    text = tempText;
                }

                if (text != string.Empty && text != "q")
                {
                    Console.WriteLine($"Dice roll: {text}");
                    var dice = text.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(a => Parse(rand, a.Trim()))
                        .ToList();
                    allDice.AddRange(dice);

                    PrintDice(dice);
                    PrintStats(allDice);
                }
            } while (text != "q");
        }

        private static List<DiceRoll> Parse(Random rand, string text)
        {
            var parts = text.Split(new[] { 'd' }, StringSplitOptions.RemoveEmptyEntries);
            var numDice = 1;
            var dice = 0;
            if (parts.Length == 2)
            {
                numDice = int.Parse(parts[0]);
                dice = int.Parse(parts[1]);
            }
            else if (parts.Length == 1 && text.StartsWith("d"))
            {
                dice = int.Parse(parts[0]);
            }

            return dice != 0 ? RollDice(rand, numDice, dice) : new List<DiceRoll>();
        }

        private static List<DiceRoll> RollDice(Random rand, int numDice, int dice)
        {
            var rolls = new List<DiceRoll>(numDice);
            for (int i = 0; i < numDice; ++i)
            {
                var roll = rand.Next(dice) + 1;
                rolls.Add(new DiceRoll { Dice = dice, Roll = roll });
            }
            return rolls;
        }

        private static void PrintDice(List<List<DiceRoll>> dice)
        {
            var flatDice = dice.SelectMany(a => a.Select(b => b.Roll)).ToList();
            if (flatDice.Count != 0)
            {
                var sum = flatDice.Sum();
                var max = flatDice.Max();
                var min = flatDice.Min();
                Console.WriteLine("dice: [" + string.Join("], [", dice.Select(d => string.Join(", ", d.Select(a => a.Roll)))) + "]");
                Console.WriteLine("sum: " + sum + " (sum of parts: " + string.Join(", ", dice.Select(d => d.Sum(a => a.Roll))) + ")");
                Console.WriteLine("max: " + max);
                Console.WriteLine("min: " + min);
                Console.WriteLine("================");
            }
        }

        private static void PrintStats(List<List<DiceRoll>> dice)
        {
            var flatDice = dice.SelectMany(a => a);
            if (flatDice.Any(a => a.Dice == 20))
            {
                var counts = new Dictionary<int, int>();
                for (int i = 1; i <= 20; ++i)
                {
                    counts[i] = 0;
                }
                foreach (var d in flatDice.Where(a => a.Dice == 20))
                {
                    counts[d.Roll] += 1;
                }
                var maxDrawHeight = 10.0;
                double maxCount = counts.Max(a => a.Value);
                var drawHeight = counts.OrderBy(a => a.Key).Select(a => Math.Round(maxDrawHeight * (a.Value / maxCount))).ToList();
                Console.WriteLine(string.Join(",", drawHeight));
                for (int i = (int)maxDrawHeight; i > 0; --i)
                {
                    Console.Write("|");
                    foreach (var height in drawHeight)
                    {
                        Console.Write(height >= i ? "+" : " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("---------------------");
                Console.WriteLine();
                Console.WriteLine("================");
            }
        }
    }
}
