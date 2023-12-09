internal abstract class HandComparer : IComparer<(string hand, int bid)>
{
    public abstract string Strengths { get; }

    public abstract HandType GetType(string hand);

    public int Compare((string hand, int bid) x, (string hand, int bid) y)
    {
        // Less than zero: x is less than y.
        // Zero: x equals y.
        // Greater than zero: x is greater than y.
        HandType xType = this.GetType(x.hand);
        HandType yType = this.GetType(y.hand);
        if (xType != yType)
        {
            return xType - yType;
        }

        for (int i = 0; i < x.hand.Length; i++)
        {
            int xStrength = this.Strengths.IndexOf(x.hand[i]);
            int yStrength = this.Strengths.IndexOf(y.hand[i]);
            if (xStrength != yStrength)
            {
                return xStrength - yStrength;
            }
        }

        return 0;
    }
}
