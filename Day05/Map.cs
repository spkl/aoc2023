namespace Day05;

public record Map(string SourceType,
                  string DestinationType,
                  List<Range> Ranges)
{
    public decimal Lookup(decimal sourceValue)
    {
        foreach (Range range in this.Ranges)
        {
            if (sourceValue < range.SourceStart)
            {
                continue;
            }

            decimal offset = sourceValue - range.SourceStart;
            if (offset < range.Length)
            {
                return range.DestinationStart + offset;
            }
        }

        return sourceValue;
    }
}