using System.Text.RegularExpressions;

namespace Day05;

public partial class Almanac
{
    private const string SeedsMarker = "seeds: ";

    public decimal[] Seeds { get; set; } = [];

    public IEnumerable<decimal> SeedsFromRanges
    {
        get
        {
            for (int i = 0; i < this.Seeds.Length - 1; i += 2)
            {
                decimal start = this.Seeds[i];
                decimal length = this.Seeds[i + 1];
                for (decimal j = 0; j < length; j++)
                {
                    yield return start + j;
                }
            }
        }
    }

    public Dictionary<(string sourceType, string destinationType), Map> Maps { get; } = [];

    internal decimal Lookup(decimal seed, IEnumerable<string> lookupSequence)
    {
        decimal value = seed;
        string source = "seed";
        foreach (string destination in lookupSequence)
        {
            value = this.Maps[(source, destination)].Lookup(value);
            source = destination;
        }

        return value;
    }

    internal static Almanac Read(string file)
    {
        Almanac almanac = new();
        Map? currentMap = null;
        foreach (string line in File.ReadLines(file))
        {
            if (line.StartsWith(SeedsMarker))
            {
                almanac.Seeds = line[SeedsMarker.Length..].Split(' ').Select(decimal.Parse).ToArray();
            }
            else if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            else if (!char.IsDigit(line[0]))
            {
                // Start of new map
                Match m = MapHeader().Match(line);
                string sourceType = m.Groups["source"].Value;
                string destinationType = m.Groups["destination"].Value;
                currentMap = new Map(sourceType, destinationType, []);
                almanac.Maps.Add((sourceType, destinationType), currentMap);
            }
            else
            {
                // Range inside a map
                if (currentMap is null)
                {
                    throw new Exception("Found range without map header.");
                }

                if (line.Split(' ').Select(decimal.Parse).ToArray() is [decimal destinationStart, decimal sourceStart, decimal length])
                {
                    currentMap.Ranges.Add(new Range(destinationStart, sourceStart, length));
                }
            }
        }

        return almanac;
    }

    [GeneratedRegex(@"(?<source>\w+)-to-(?<destination>\w+) map:")]
    private static partial Regex MapHeader();
}