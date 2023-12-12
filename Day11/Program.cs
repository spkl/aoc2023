internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(@"..\..\..\input.txt");

        SumShortestPaths(lines, 2);
        SumShortestPaths(lines, 1000000);
    }

    private static void SumShortestPaths(string[] lines, long growFactor)
    {
        List<Galaxy> galaxies = ReadGalaxies(lines);

        ExpandUniverse(galaxies, growFactor);
        
        (Galaxy, Galaxy)[] pairs = GetPairs(galaxies).ToArray();
        long shortestPathSum = pairs.Select(pair => GetShortestPathLength(pair.Item1, pair.Item2)).Sum();

        Console.WriteLine($"When growing by factor {growFactor}, the sum of shortest paths is {shortestPathSum}");
    }

    private static List<Galaxy> ReadGalaxies(string[] lines)
    {
        List<Galaxy> galaxies = [];
        int number = 1;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] is '#')
                {
                    galaxies.Add(new Galaxy(number++, new Position(x, y)));
                }
            }
        }

        return galaxies;
    }

    private static void ExpandUniverse(List<Galaxy> galaxies, long factor)
    {
        long growBy = factor - 1;
        long maxX = galaxies.Select(g => g.Position.X).Max();
        for (long x = 0; x <= maxX; x++)
        {
            bool isEmpty = galaxies.All(g => g.Position.X != x);
            if (isEmpty)
            {
                foreach (Galaxy galaxy in galaxies)
                {
                    if (galaxy.Position.X > x)
                    {
                        galaxy.Position.X += growBy;
                    }
                }

                x += growBy;
                maxX += growBy;
            }
        }

        long maxY = galaxies.Select(g => g.Position.Y).Max();
        for (long y = 0; y <= maxY; y++)
        {
            bool isEmpty = galaxies.All(g => g.Position.Y != y);
            if (isEmpty)
            {
                foreach (Galaxy galaxy in galaxies)
                {
                    if (galaxy.Position.Y > y)
                    {
                        galaxy.Position.Y += growBy;
                    }
                }
                
                y += growBy;
                maxY += growBy;
            }
        }        
    }

    private static IEnumerable<(Galaxy, Galaxy)> GetPairs(List<Galaxy> galaxies)
    {
        foreach (Galaxy g1 in galaxies)
        {
            foreach (Galaxy g2 in galaxies)
            {
                if (g1.Number < g2.Number)
                {
                    yield return (g1, g2);
                }
            }
        }
    }

    private static long GetShortestPathLength(Galaxy item1, Galaxy item2)
    {
        return Math.Abs(item1.Position.X - item2.Position.X) + Math.Abs(item1.Position.Y - item2.Position.Y);
    }

    private static void Print(List<Galaxy> galaxies)
    {
        long maxX = galaxies.Select(g => g.Position.X).Max();
        long maxY = galaxies.Select(g => g.Position.Y).Max();
        for (long y = 0; y <= maxY; y++)
        {
            for (long x = 0; x <= maxX; x++)
            {
                Console.Write(galaxies.Any(g => g.Position.X == x && g.Position.Y == y) ? "#" : ".");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}