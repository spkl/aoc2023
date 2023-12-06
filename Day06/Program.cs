internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(@"..\..\..\input.txt");
        
        long[] raceTimes = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToArray();
        long[] recordDistances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToArray();

        long productMulti = GetPossibilitiesProduct(raceTimes, recordDistances);
        Console.WriteLine($"Product for multiple races: {productMulti}");

        long singleRaceTime = long.Parse(string.Join("", raceTimes));
        long singleRecordDistance = long.Parse(string.Join("", recordDistances));

        long productSingle = GetPossibilitiesProduct([singleRaceTime], [singleRecordDistance]);
        Console.WriteLine($"Product for single race: {productSingle}");
    }

    private static long GetPossibilitiesProduct(long[] raceTimes, long[] recordDistances)
    {
        long product = 1;
        for (long i = 0; i < raceTimes.Length; i++)
        {
            long raceTime = raceTimes[i];
            long recordDistance = recordDistances[i];
            long possibilities = 0;
            for (long buttonTime = 0; buttonTime <= raceTime; buttonTime++)
            {
                long myDistance = buttonTime * (raceTime - buttonTime);
                if (myDistance > recordDistance)
                {
                    possibilities++;
                }
            }

            product *= possibilities;
        }

        return product;
    }
}