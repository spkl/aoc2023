internal class PartTwoComparer : HandComparer
{
    public override string Strengths { get; } = "J23456789TQKA";

    private readonly PartOneComparer partOneComparer = new();

    public override HandType GetType(string hand)
    {
        // Approach: Replace jokers with every possible other card type and take the best result.
        return this.Strengths.Except(['J']).Select(card => hand.Replace('J', card)).Select(this.partOneComparer.GetType).Max();
    }
}