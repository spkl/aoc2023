internal class PartOneComparer : HandComparer
{
    public override string Strengths { get; } = "23456789TJQKA";

    public override HandType GetType(string hand)
    {
        int[] counts = hand.Distinct().Select(card => hand.Count(card2 => card == card2)).OrderDescending().ToArray();
        return counts switch
        {
            [5] => HandType.FiveOfAKind,
            [4, 1] => HandType.FourOfAKind,
            [3, 2] => HandType.FullHouse,
            [3, 1, 1] => HandType.ThreeOfAKind,
            [2, 2, 1] => HandType.TwoPair,
            [2, 1, 1, 1] => HandType.OnePair,
            _ => HandType.HighCard,
        };
    }
}