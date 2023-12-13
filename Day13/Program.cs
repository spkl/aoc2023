
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

        int result1 = 0;
        int result2 = 0;
        foreach (List<string> pattern in patterns)
        {
            int partialResult1 = FindReflection(pattern);
            result1 += partialResult1;

            for (int row = 0; row < pattern.Count; row++)
            {
                for (int col = 0; col < pattern[row].Length; col++)
                {
                    List<string> changedPattern = [.. pattern];
                    char[] chars = changedPattern[row].ToCharArray();
                    chars[col] = chars[col] is '#' ? '.' : '#';
                    changedPattern[row] = new string(chars);

                    int partialResult2 = FindReflection(changedPattern, partialResult1);
                    if (partialResult2 != -1)
                    {
                        result2 += partialResult2;
                        row = int.MaxValue - 1;
                        break;
                    }
                }
            }
        }

        Console.WriteLine(result1);
        Console.WriteLine(result2);
    }

    private static int FindReflection(List<string> pattern, int unallowedResult = -1)
    {
        if (FindVerticalReflection(pattern, out int column) && column != unallowedResult)
        {
            return column;
        }
        else if (FindHorizontalReflection(pattern, out int row) && 100 * row != unallowedResult)
        {
            return 100 * row;
        }
        else
        {
            return -1;
        }
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