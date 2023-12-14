
internal class Program
{
    private const string Path = @"..\..\..\input.txt";

    private static void Main(string[] args)
    {
        char[,] map = ReadMap();

        // Part 1
        Tilt(map, -1, 0);
        Console.WriteLine(CalculateNorthLoad(map));

        // Part 2
        List<int> loads = [];
        for (int i = 0; i < 200; i++)
        {
            SpinCycle(map);
            loads.Add(CalculateNorthLoad(map));
        }

        (int cycleStart, List<int> loadCycle) = FindCycle(loads);
        int targetIndex = (1000000000 - cycleStart - 1) % loadCycle.Count;
        Console.WriteLine(loadCycle[targetIndex]);
    }

    private static (int cycleStart, List<int> cycle) FindCycle(List<int> loads)
    {
        for (int cycleStart = 0; cycleStart < loads.Count; cycleStart++)
        {
            for (int cycleLength = 2; cycleLength < (loads.Count - cycleStart) / 2; cycleLength++)
            {
                int offset = 1;
                bool isCycle = true;
                while (isCycle && cycleStart + cycleLength * offset < loads.Count)
                {
                    for (int pos = 0; pos < cycleLength && cycleStart + offset * cycleLength + pos < loads.Count; pos++)
                    {
                        if (loads[cycleStart + pos] != loads[cycleStart + offset * cycleLength + pos])
                        {
                            isCycle = false;
                            break;
                        }
                    }

                    offset++;
                }

                if (isCycle)
                {
                    return (cycleStart, loads.Skip(cycleStart).Take(cycleLength).ToList());
                }
            }
        }

        throw new Exception("Found no cycle");
    }

    private static char[,] ReadMap()
    {
        string[] lines = File.ReadAllLines(Path);
        char[,] map = new char[lines.Length, lines[0].Length];
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[y, x] = lines[y][x];
            }
        }

        return map;
    }

    private static void Tilt(char[,] map, int vy, int vx)
    {
        bool down = vy > 0;
        bool right = vx > 0;
        foreach (int y in Iterate(0, map.GetLength(0) - 1, down))
        {
            foreach (int x in Iterate(0, map.GetLength(1) - 1, right))
            {
                if (map[y, x] != 'O')
                {
                    continue;
                }

                int oy = y + vy;
                int ox = x + vx;
                while (oy >= 0 && oy < map.GetLength(0)
                    && ox >= 0 && ox < map.GetLength(1)
                    && map[oy, ox] == '.')
                {
                    map[oy - vy, ox - vx] = '.';
                    map[oy, ox] = 'O';

                    oy += vy;
                    ox += vx;
                }
            }
        }
    }

    private static void SpinCycle(char[,] map)
    {
        Tilt(map, -1, 0);
        Tilt(map, 0, -1);
        Tilt(map, 1, 0);
        Tilt(map, 0, 1);
    }

    private static int CalculateNorthLoad(char[,] map)
    {
        int load = 0;

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 'O')
                {
                    load += map.GetLength(0) - y;
                }
            }
        }

        return load;
    }

    private static IEnumerable<int> Iterate(int from, int to, bool reverse)
    {
        if (reverse)
        {
            for (int i = to; i >= from; i--)
            {
                yield return i;
            }
        }
        else
        {
            for (int i = from; i <= to; i++)
            {
                yield return i;
            }
        }
    }

    private static void Print(char[,] map)
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            Console.WriteLine(Enumerable.Range(0, map.GetLength(1)).Select(x => map[y, x]).ToArray());
        }
    }
}