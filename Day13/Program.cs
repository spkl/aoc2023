
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        List<List<string>> patterns = [];
        List<string> currentPattern = [];
        patterns.Add(currentPattern);

        foreach (string line in File.ReadLines(@"..\..\..\input.txt"))
        {
            if (string.IsNullOrEmpty(line))
            {
                currentPattern = [];
                patterns.Add(currentPattern);
                continue;
            }

            currentPattern.Add(line);
        }

        int result = 0;
        foreach (List<string> pattern in patterns)
        {
            if (FindVerticalReflection(pattern, out int column))
            {
                result += column;
            }
            else if (FindHorizontalReflection(pattern, out int row))
            {
                result += 100 * row;
            }
            else
            {
                throw new Exception("Could not find pattern.");
            }
        }

        Console.WriteLine(result);
    }

    private static bool FindHorizontalReflection(List<string> pattern, out int row)
    {
        for (int i = 0; i <= pattern.Count - 2; i++)
        {
            string current = pattern[i];
            string next = pattern[i + 1];
            if (current != next)
            {
                continue;
            }

            int decIndex = i;
            int incIndex = i + 1;
            bool isReflection = true;
            while (decIndex >= 0 && incIndex < pattern.Count)
            {
                if (pattern[decIndex] != pattern[incIndex])
                {
                    isReflection = false;
                    break;
                }

                decIndex--;
                incIndex++;
            }

            if (isReflection)
            {
                row = i + 1;
                return true;
            }
        }

        row = -1;
        return false;
    }

    private static bool FindVerticalReflection(List<string> pattern, out int column)
    {
        List<string> rotatedPattern = [];
        for (int x = 0; x < pattern[0].Length; x++)
        {
            StringBuilder builder = new(pattern.Count);
            for (int y = 0; y < pattern.Count; y++)
            {
                builder.Append(pattern[y][x]);
            }

            rotatedPattern.Add(builder.ToString());
        }
        
        return FindHorizontalReflection(rotatedPattern, out column);
    }
}