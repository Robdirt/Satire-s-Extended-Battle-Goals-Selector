using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NumberGenerator
{
    internal class Generator
    {
        private static void Main(string[] args)
        {
            const int maxSize = 79;
            string outputId = "draw size: ";
            int outputSize = 3;

            try
            {
                string fileName = Path.Combine(Environment.CurrentDirectory, "BattleGoals.txt");

                if (args.Length > 1
                    && args[0].Equals("clear", StringComparison.CurrentCultureIgnoreCase)
                    && File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                List<string> lines = new List<string>();
                if (File.Exists(fileName))
                {
                    lines.AddRange(File.ReadAllText(fileName).Split('\n'));
                }

                if (lines.FirstOrDefault()?.Contains(outputId) ?? false)
                {
                    outputSize = int.Parse(lines.First().Replace(outputId, string.Empty));
                }
                else
                {
                    bool isValid = false;
                    do
                    {
                        Console.Write("How many battle goals do you want to draw? ");
                        string value = Console.ReadLine();
                        if (value.All(char.IsDigit))
                        {
                            int number = int.Parse(value);
                            if (number > 0 && number < maxSize / 2)
                            {
                                isValid = true;
                                outputSize = number;
                            }
                        }
                        if (!isValid)
                            Console.WriteLine("Invalid Input try again.\n");
                    } while (!isValid);
                }

                List<int> inputSet = lines.Any()
                                    ? lines.Where(line => line.All(char.IsDigit)).Select(num => int.Parse(num)).ToList()
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

                    File.WriteAllText(Path.Combine(fileName, fileName), $"{outputId}{outputSize}\n{string.Join("\n", inputSet.Concat(myPoints))}");

                    Console.WriteLine("Drawn cards will not be drawn again.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Not Enough cards in the deck to draw {outputSize} cards. Log File will be deleted.");
                    File.Delete(fileName);
                    File.WriteAllText(Path.Combine(fileName, fileName), $"{outputId}{outputSize}");
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
