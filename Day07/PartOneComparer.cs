internal class PartOneComparer : IComparer<(string hand, int bid)>
{
    private static readonly Dictionary<char, int> Strengths = new()
    {
        {'A', 14},
        {'K', 13},
        {'Q', 12},
        {'J', 11},
        {'T', 10},
        {'9', 9},
        {'8', 8},
        {'7', 7},
        {'6', 6},
        {'5', 5},
        {'4', 4},
        {'3', 3},
        {'2', 2},
    };

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
            char xCard = x.hand[i];
            char yCard = y.hand[i];
            if (Strengths[xCard] != Strengths[yCard])
            {
                return Strengths[xCard] - Strengths[yCard];
            }
        }

        return 0;
    }

    private HandType GetType(string hand)
    {
        Dictionary<char, int> counts = [];
        foreach (char card in hand)
        {
            if (!counts.ContainsKey(card))
            {
                counts[card] = 0;
            }

            counts[card]++;
        }

        if (counts.Count == 1)
        {
            return HandType.FiveOfAKind;
        }
        else if (counts.ContainsValue(4))
        {
            return HandType.FourOfAKind;
        }
        else if (counts.ContainsValue(3) && counts.ContainsValue(2))
        {
            return HandType.FullHouse;
        }
        else if (counts.ContainsValue(3))
        {
            return HandType.ThreeOfAKind;
        }
        else if (counts.Values.Count(count => count == 2) == 2)
        {
            return HandType.TwoPair;
        }
        else if (counts.Values.Any(count => count == 2))
        {
            return HandType.OnePair;
        }
        else 
        {
            return HandType.HighCard;
        }
    }
}