internal class PartTwoComparer : IComparer<(string hand, int bid)>
{
    private static readonly Dictionary<char, int> Strengths = new()
    {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'T', 10},
        {'9', 9},
        {'8', 8},
        {'7', 7},
        {'6', 6},
        {'5', 5},
        {'4', 4},
        {'3', 3},
        {'2', 2},
        {'J', 1},
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

    internal HandType GetType(string hand)
    {
        int jokers = 0;
        Dictionary<char, int> counts = [];
        foreach (char card in hand)
        {
            if (card == 'J')
            {
                jokers++;
                continue;
            }

            if (!counts.ContainsKey(card))
            {
                counts[card] = 0;
            }

            counts[card]++;
        }

        if (counts.Count == 1 || jokers == 5)
        {
            return HandType.FiveOfAKind;
        }
        // ! From this point on, there are at most 3 jokers.
        else if (counts.Values.Max() + jokers == 4)
        {
            return HandType.FourOfAKind;
        }
        // ! From this point on, there are at most 2 jokers.
        else if (jokers == 2 && counts.ContainsValue(2)
                 || jokers == 1 && counts.Values.Count(count => count == 2) == 2
                 || counts.ContainsValue(3) && counts.ContainsValue(2))
        {
            return HandType.FullHouse;
        }
        else if (counts.Values.Max() + jokers == 3)
        {
            return HandType.ThreeOfAKind;
        }
        // ! From this point on, there is at most 1 joker.
        else if (counts.Values.Count(count => count == 2) == 2 
                 || counts.Values.Count(count => count == 2) == 1 && jokers > 0)
        {
            return HandType.TwoPair;
        }
        else if (counts.Values.Any(count => count == 2) || jokers > 0)
        {
            return HandType.OnePair;
        }
        else 
        {
            return HandType.HighCard;
        }
    }
}