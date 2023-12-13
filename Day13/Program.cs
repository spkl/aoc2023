
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

        int sum1 = 0;
        int sum2 = 0;
        foreach (List<string> pattern in patterns)
        {
            FindResult result1 = FindReflection(pattern, FindResult.None());
            result1.AddToSum(ref sum1);

            for (int row = 0; row < pattern.Count; row++)
            {
                for (int col = 0; col < pattern[row].Length; col++)
                {
                    List<string> changedPattern = [.. pattern];
                    char[] chars = changedPattern[row].ToCharArray();
                    chars[col] = chars[col] is '#' ? '.' : '#';
                    changedPattern[row] = new string(chars);

                    FindResult result2 = FindReflection(changedPattern, result1);
                    if (result2.Success)
                    {
                        result2.AddToSum(ref sum2);
                        row = int.MaxValue - 1;
                        break;
                    }
                }
            }
        }

        Console.WriteLine(sum1);
        Console.WriteLine(sum2);
    }

    private static FindResult FindReflection(List<string> pattern, FindResult ignore)
    {
        if (FindVerticalReflection(pattern, ignore.Column, out int column))
        {
            return FindResult.InColumn(column);
        }
        else if (FindHorizontalReflection(pattern, ignore.Row, out int row))
        {
            return FindResult.InRow(row);
        }
        
        return FindResult.None();
    }

    private static bool FindHorizontalReflection(List<string> pattern, int ignoreRow, out int row)
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

            row = i + 1;
            if (isReflection && ignoreRow != row)
            {
                return true;
            }
        }

        row = -1;
        return false;
    }

    private static bool FindVerticalReflection(List<string> pattern, int ignoreColumn, out int column)
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
        
        return FindHorizontalReflection(rotatedPattern, ignoreColumn, out column);
    }
}