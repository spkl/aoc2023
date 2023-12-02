using System.Text.RegularExpressions;

internal partial class Program
{
    static Dictionary<string, int> limits = new(){
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };

    private static void Main(string[] args)
    {
        int possibleGameIdSum = 0;
        int powers = 0;
        
        foreach (string game in File.ReadLines(@"..\..\..\input.txt"))
        {
            bool gamePossible = true;

            Match match = GameRegex().Match(game);
            int gameId = int.Parse(match.Groups["id"].Value);
            string[] sets = match.Groups["sets"].Value.Split("; ");

            Dictionary<string, int> minCubes = new(){
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 }
            };

            foreach (string set in sets)
            {
                string[] cubes = set.Split(", ");
                foreach (string cube in cubes)
                {
                    if (cube.Split(" ") is not ([string numberStr, string color]))
                    {
                        throw new Exception($"Could not parse '{cube}'");
                    }

                    int number = int.Parse(numberStr);
                    if (number > limits[color])
                    {
                        gamePossible = false;
                    }

                    minCubes[color] = Math.Max(minCubes[color], number);
                }
            }

            int power = minCubes.Values.Aggregate((x, y) => x * y);
            powers += power;

            if (gamePossible)
            {
                possibleGameIdSum += gameId;
            }
        }

        Console.WriteLine($"Sum of possible game IDs: {possibleGameIdSum}");
        Console.WriteLine($"Sum of powers: {powers}");
    }

    [GeneratedRegex(@"^Game (?<id>\d+): (?<sets>.*)")]
    private static partial Regex GameRegex();
}