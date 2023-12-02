using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        const string patternWithoutWords = @"(?<digit>\d)";
        const string patternWithWords = @"(?=(?<digit>\d|one|two|three|four|five|six|seven|eight|nine))"; // Use lookahead to handle cases like "twone" (=> should match two, one)

        int sum = 0;
        string pattern = patternWithWords;
        string[] lines = File.ReadAllLines(@"..\..\..\input.txt");

        foreach (string line in lines)
        {
            MatchCollection matches = Regex.Matches(line, pattern);
            if (matches.Count == 0)
            {
                throw new Exception($"Found no first/last digit in line '{line}'");
            }

            string firstDigit = GetDigit(matches[0].Groups["digit"].Value);
            string secondDigit = GetDigit(matches[^1].Groups["digit"].Value);
            sum += int.Parse(firstDigit + secondDigit);
        }

        Console.WriteLine(sum);
    }

    private static string GetDigit(string value)
    {
        if (value.Length == 1)
        {
            return value;
        }

        return value switch
        {
            "one" => "1",
            "two" => "2",
            "three" => "3",
            "four" => "4",
            "five" => "5",
            "six" => "6",
            "seven" => "7",
            "eight" => "8",
            "nine" => "9",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}