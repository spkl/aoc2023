internal class Program
{
    private const string Path = @"..\..\..\input.txt";

    private static void Main(string[] args)
    {
        Console.WriteLine(CalculatePossibilitiesSum(1));
        Console.WriteLine(CalculatePossibilitiesSum(5));
    }

    private static int CalculatePossibilitiesSum(int repeat)
    {
        object sumLock = new();
        int possibilitiesSum = 0;
        
        Parallel.ForEach(File.ReadLines(Path), line => 
        {
            if (line.Split(' ') is not ([string state, string damagedGroupsString]))
            {
                throw new Exception($"Could not parse {line}");
            }

            state = string.Join("?", Enumerable.Repeat(state, repeat));
            damagedGroupsString = string.Join(",", Enumerable.Repeat(damagedGroupsString, repeat));

            int[] damagedGroups = damagedGroupsString.Split(',').Select(int.Parse).ToArray();

            int possibilities = 0;
            foreach (string possibleState in GetPossibleStates(state))
            {
                if (MatchesDamagedGroups(possibleState, damagedGroups))
                {
                    possibilities++;
                }
            }

            lock (sumLock)
            {
                possibilitiesSum += possibilities;
            }
        });

        return possibilitiesSum;
    }

    private static IEnumerable<string> GetPossibleStates(string recordedState)
    {
        int unknowns = recordedState.Count(c => c is '?');
        int possibilities = 2 << (unknowns - 1);
        
        int[] unknownIndexes = new int[unknowns];
        int unknownIndex = 0;
        for (int i = 0; i < recordedState.Length; i++)
        {
            if (recordedState[i] is '?')
            {
                unknownIndexes[unknownIndex] = i;
                unknownIndex++;
            }
        }

        char[] possibleState = [.. recordedState];
        for (int bitfield = 0; bitfield < possibilities; bitfield++)
        {
            for (int bit = 0; bit < unknowns; bit++)
            {
                possibleState[unknownIndexes[bit]] = ((bitfield >> bit) & 1) == 1 ? '#' : '.';
            }

            yield return new string(possibleState);
        }
    }

    private static bool MatchesDamagedGroups(string state, int[] damagedGroups)
    {
        int groupIndex = 0;
        int damagedCount = 0;
        foreach (char spring in state)
        {
            switch (spring)
            {
                case '#':
                    if (groupIndex > damagedGroups.Length - 1)
                    {
                        return false;
                    }

                    damagedCount++;
                    if (damagedCount > damagedGroups[groupIndex])
                    {
                        return false;
                    }
                    break;
                case '.':
                    if (damagedCount > 0)
                    {
                        if (damagedCount != damagedGroups[groupIndex])
                        {
                            return false;
                        }

                        groupIndex++;
                        damagedCount = 0;
                    }
                    break;
                default:
                    throw new Exception($"Unknown character {spring}");
            }
        }

        if (damagedCount > 0)
        {
            if (damagedCount != damagedGroups[groupIndex])
            {
                return false;
            }

            groupIndex++;
        }
        
        if (groupIndex != damagedGroups.Length)
        {
            return false;
        }

        return true;
    }

    private static bool MatchesDamagedGroups2(string state, int[] damagedGroups)
    {
        return state.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Length).SequenceEqual(damagedGroups);
    }
}