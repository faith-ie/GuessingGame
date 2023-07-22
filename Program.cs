using System;
using System.IO;
using System.Xml;

namespace GuessingGame
{
    internal static class Program
    {
        internal static void Main() => Guess();

        private static void Guess()
        {
            int minValue = 1;
            int maxValue = 10;
            int maxTries = 5;
            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load("../../../config.xml");
                XmlNode? documentElement = xmlDoc.DocumentElement;

                if (documentElement != null)
                {
                    XmlNodeList settingsNodes = documentElement.ChildNodes;
                    foreach (XmlNode node in settingsNodes)
                    {
                        if (Enum.TryParse<Config>(node.Name, out var setting))
                        {
                            switch (setting)
                            {
                                case Config.MinValue:
                                    minValue = int.Parse(node.InnerText);
                                    break;

                                case Config.MaxValue:
                                    maxValue = int.Parse(node.InnerText);
                                    break;

                                case Config.MaxTries:
                                    maxTries = int.Parse(node.InnerText);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error reading settings from config.xml: " + ex.Message);
                Console.WriteLine("Using default values instead.");
            }
            Random rnd = new();
            int e = rnd.Next(minValue, maxValue);
            int counter = maxTries;
            Console.WriteLine("Welcome to my guessing game!");
            Console.WriteLine($"Try guessing a number from {minValue}-{maxValue} in {maxTries} tries or less!");
            while (true)
            {
                string? g = Console.ReadLine();
                int.TryParse(g, out int n);
                if (!string.IsNullOrEmpty(g) || !string.IsNullOrWhiteSpace(g))
                {
                    if (counter > 0)
                    {
                        if (n != e)
                        {
                            counter--;
                            string hint = (n > e) ? "lower" : "higher";
                            Console.WriteLine($"It's {hint}");
                            Console.WriteLine($"You have {counter} {(counter == 1 ? "try" : "tries")} left.");
                        }
                        else
                        {
                            Console.WriteLine("Congratulations! You guessed it!");
                            Console.WriteLine($"Number: {e}");
                            Console.WriteLine($"Tries left: {counter}");
                            break;
                        }
                    }
                    if (counter == 0)
                    {
                        Console.WriteLine("Oh no! You ran out of guesses :(");
                        Console.WriteLine($"The number was {e}");
                        break;
                    }
                }
            }
        }

        private enum Config
        {
            MinValue,
            MaxValue,
            MaxTries
        }
    }
}