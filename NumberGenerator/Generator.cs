using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NumberGenerator
{
    class Generator
    {
        static void Main(string[] args)
        {
            const int maxSize = 79;
            const int outputSize = 3;

            try
            {
                string fileName = Path.Combine(Environment.CurrentDirectory, "BattleGoals.txt");
                if (args.Length > 1
                    && args[0].Equals("clear", StringComparison.CurrentCultureIgnoreCase)
                    && File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                List<int> inputSet = File.Exists(fileName)
                                    ? File.ReadAllText(fileName).Split('\n').Select(num => int.Parse(num)).ToList()
                                    : new List<int>();

                Console.WriteLine($"{inputSet.Count} Battle Goals have previously been drawn.");

                if (inputSet.Count + outputSize < maxSize)
                {

                    Random myNumber = new Random(DateTime.Now.TimeOfDay.Milliseconds);
                    HashSet<int> myPoints = new HashSet<int>();

                    while (myPoints.Count < outputSize)
                    {
                        var number = myNumber.Next(1, maxSize);
                        if (!inputSet.Contains(number))
                            myPoints.Add(number);
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Battle goal options:  {string.Join(", ", myPoints)}");
                    Console.WriteLine();

                    File.WriteAllText(Path.Combine(fileName, fileName), string.Join("\n", inputSet.Concat(myPoints)));

                    Console.WriteLine("Drawn cards will not be drawn again.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Not Enough cards in the deck to draw {outputSize} cards. Log File will be deleted.");
                    File.Delete(fileName);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
            Console.Write("Press Enter to Exit.");
            Console.ReadLine();
        }
    }
}
