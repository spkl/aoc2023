internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(@"..\..\..\input.txt");
        char[,] map = new char[lines.Length, lines[0].Length];
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[y, x] = lines[y][x];
            }
        }

        Tilt(map, -1, 0);
        Console.WriteLine(CalculateNorthLoad(map));
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