internal class Program
{
    private const string Path = @"..\..\..\input.txt";

    private static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part1()
    {
        int sum = 0;
        foreach (string card in File.ReadLines(Path))
        {
            int matches = GetMatches(card);
            if (matches > 0)
            {
                sum += 1 << (matches - 1);
            }
        }

        Console.WriteLine($"Part 1: {sum}");
    }

    private static void Part2()
    {
        string[] cards = File.ReadAllLines(Path);
        int[] cardAmounts = Enumerable.Repeat(1, cards.Length).ToArray();
        for (int i = 0; i < cardAmounts.Length; i++)
        {
            int matches = GetMatches(cards[i]);
            for (int j = i + 1; j <= i + matches && j < cardAmounts.Length; j++)
            {
                cardAmounts[j] += cardAmounts[i];
            }
        }

        Console.WriteLine($"Part 2: {cardAmounts.Sum()}");
    }

    private static int GetMatches(string card)
    {
        if (card.Split(':') is not ([_, string numbersStr])
            || numbersStr.Split('|') is not ([string winningNumbersStr, string haveNumbersStr]))
        {
            throw new Exception($"Error parsing input '{card}'");
        }

        int[] winningNumbers = winningNumbersStr.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        int[] haveNumbers = haveNumbersStr.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        return haveNumbers.Count(number => winningNumbers.Contains(number));
    }
}