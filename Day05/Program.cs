namespace Day05;

internal partial class Program
{
    private static void Main(string[] args)
    {
        Almanac almanac = Almanac.Read(@"..\..\..\input.txt");

        string[] lookupSequence = ["soil", "fertilizer", "water", "light", "temperature", "humidity", "location"];

        decimal lowestLocation1 = int.MaxValue;
        foreach (decimal seed in almanac.Seeds)
        {
            lowestLocation1 = Math.Min(lowestLocation1, almanac.Lookup(seed, lookupSequence));
        }
        
        object l = new();
        decimal lowestLocation2 = int.MaxValue;
        Parallel.ForEach(almanac.SeedsFromRanges, seed => 
        {
            decimal location = almanac.Lookup(seed, lookupSequence);
            lock (l)
            {
                lowestLocation2 = Math.Min(lowestLocation2, location); 
            }
        });

        Console.WriteLine($"Lowest location 1: {lowestLocation1}");
        Console.WriteLine($"Lowest location 2: {lowestLocation2}");
    }
    
}