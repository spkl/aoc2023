internal class Program
{
    private static void Main(string[] args)
    {
        int[][] histories = File.ReadLines(@"..\..\..\input.txt")
                                .Select(line => line.Split(" "))
                                .Select(strings => strings.Select(int.Parse).ToArray())
                                .ToArray();

        int nextValuesSum = histories.Select(FindNextValue).Sum();
        Console.WriteLine(nextValuesSum);

        int previousValuesSum = histories.Select(FindPreviousValue).Sum();
        Console.WriteLine(previousValuesSum);
    }

    private static int FindNextValue(int[] values)
    {
        int[][] allDifferences = CalculateAllDifferences(values).ToArray();

        int nextDiff = 0;
        foreach (int[] differences in allDifferences.Reverse().Skip(1))
        {
            nextDiff = differences.Last() + nextDiff;
        }

        return values.Last() + nextDiff;
    }

    private static int FindPreviousValue(int[] values)
    {
        return FindNextValue(values.Reverse().ToArray());
    }

    private static IEnumerable<int[]> CalculateAllDifferences(int[] values)
    {
        int[] lastDifferences = values;
        do
        {
            lastDifferences = CalculateDifferences(lastDifferences);
            yield return lastDifferences;
        } while (lastDifferences.Any(d => d != 0));
    }

    private static int[] CalculateDifferences(int[] values)
    {
        int[] differences = new int[values.Length - 1];
        for (int i = 0; i < values.Length - 1; i++)
        {
            differences[i] = values[i + 1] - values[i];
        }
        
        return differences;
    }
}