internal class Program
{
    private static void Main(string[] args)
    {
        List<(string hand, int bid)> hands = [];
        foreach (string line in File.ReadLines(@"..\..\..\input.txt"))
        {
            if (line.Split(" ") is [string hand, string bidString])
            {
                hands.Add((hand, int.Parse(bidString)));
            }
        }

        hands.Sort(new PartOneComparer());
        long winnings1 = CalculateWinnings(hands);
        
        hands.Sort(new PartTwoComparer());
        long winnings2 = CalculateWinnings(hands);

        Console.WriteLine(winnings1);
        Console.WriteLine(winnings2);
    }

    private static long CalculateWinnings(List<(string hand, int bid)> hands)
    {
        long winnings = 0;
        int rank = 1;
        foreach ((_, int bid) in hands)
        {
            winnings += rank * bid;
            rank++;
        }

        return winnings;
    }
}