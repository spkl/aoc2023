internal partial class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(@"..\..\..\input.txt");
        int sum = 0;
        Dictionary<(int i, int j), List<int>> gearCandidates = [];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int j = 0;
            while (ScanForNextNumber(lines, i, ref j, out NumberResult result))
            {
                if (IsAdjacentToSymbol(lines, result))
                {
                    sum += result.Number;
                }

                foreach ((int i, int j) candidate in GetAdjacentGearCandidates(lines, result))
                {
                    if (!gearCandidates.TryGetValue(candidate, out List<int>? numberList))
                    {
                        numberList = [];
                        gearCandidates.Add(candidate, numberList);
                    }

                    numberList.Add(result.Number);
                }
            }
        }

        Console.WriteLine($"Sum of part numbers: {sum}");
        
        IEnumerable<List<int>> gears = gearCandidates.Values.Where(list => list.Count == 2);
        IEnumerable<int> gearRatios = gears.Select(list => list[0] * list[1]);
        Console.WriteLine($"Sum of gear ratios: {gearRatios.Sum()}");
    }

    private static bool ScanForNextNumber(string[] lines, int i, ref int j, out NumberResult result)
    {
        string bucket = "";
        string line = lines[i];
        while (j < line.Length)
        {
            if (line[j] is >= '0' and <= '9')
            {
                bucket += line[j];
            }
            else if (bucket.Length > 0)
            {
                break;
            }
            
            j++;
        }

        if (bucket.Length > 0)
        {
            result = new NumberResult()
            {
                Number = int.Parse(bucket),
                RowIndex = i,
                FirstColIndex = j - bucket.Length,
                LastColIndex = j - 1,
            };

            return true;
        }

        result = new NumberResult();
        return false;
    }

    private static bool IsAdjacentToSymbol(string[] lines, NumberResult result)
    {
        bool isAdjacentToSymbol = false;
        IterateAdjacentArea(lines, result, (i, j) =>
            {
                isAdjacentToSymbol |= lines[i][j] is not (>= '0' and <= '9') and not '.';
            });
        
        return isAdjacentToSymbol;
    }
    private static List<(int i, int j)> GetAdjacentGearCandidates(string[] lines, NumberResult result)
    {
        List<(int i, int j)> adjacentGearCandidates = [];
        IterateAdjacentArea(lines, result, (i, j) =>
            {
                if (lines[i][j] is '*')
                {
                    adjacentGearCandidates.Add((i, j));
                }
            });

        return adjacentGearCandidates;
    }

    private static void IterateAdjacentArea(string[] lines, NumberResult result, Action<int, int> action)
    {
        (int i, int j) from = (result.RowIndex - 1, result.FirstColIndex - 1);
        (int i, int j) to = (result.RowIndex + 1, result.LastColIndex + 1);
        for (int i = from.i; i <= to.i; i++)
        {
            if (i < 0 || i >= lines.Length)
            {
                continue;
            }

            for (int j = from.j; j <= to.j; j++)
            {
                if (j < 0 || j >= lines[i].Length)
                {
                    continue;
                }

                action(i, j);
            }
        }
    }
}