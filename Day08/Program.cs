internal class Program
{
    private const string Path = @"..\..\..\input.txt";

    private static void Main(string[] args)
    {
        string instructions;
        Dictionary<string, (string left, string right)> network;
        (instructions, network) = ReadFile();

        int steps1 = CalculatePart1(instructions, network);
        Console.WriteLine(steps1);
        
        long steps2 = CalculatePart2(instructions, network);
        Console.WriteLine(steps2);
    }

    private static int CalculatePart1(string instructions, Dictionary<string, (string left, string right)> network)
    {
        string location = "AAA";
        const string target = "ZZZ";
        int steps = 0;
        foreach (char instruction in Repeat(instructions))
        {
            if (location == target)
            {
                break;
            }

            location = instruction is 'R' ? network[location].right : network[location].left;
            steps++;
        }

        return steps;
    }

    private static long CalculatePart2(string instructions, Dictionary<string, (string left, string right)> network)
    {
        string[] locations = network.Keys.Where(o => o.EndsWith('A')).ToArray();
        int[] cycleLengths = locations.Select(x => FindCycleLength(x, instructions, network)).ToArray();
        return cycleLengths.Select(x => (long)x).Aggregate(LeastCommonMultiple);
    }

    private static int FindCycleLength(string location, string instructions, Dictionary<string, (string left, string right)> network)
    {
        List<int> stepsToEnd = [];
        int steps = 0;
        foreach (char instruction in Repeat(instructions))
        {
            if (location.EndsWith('Z'))
            {
                if (stepsToEnd.Any(o => o == steps))
                {
                    return steps;
                }

                stepsToEnd.Add(steps);
                steps = 0;
            }

            location = instruction is 'R' ? network[location].right : network[location].left;
            steps++;
        }

        return -1;
    }

    private static (string instructions, Dictionary<string, (string left, string right)> network) ReadFile()
    {
        string[] lines = File.ReadAllLines(Path);

        string instructions = lines[0];
        Dictionary<string, (string left, string right)> network = [];
        foreach (string line in lines.Skip(2))
        {
            string name = line[0..3];
            string left = line[7..10];
            string right = line[12..15];
            network[name] = (left, right);
        }

        return (instructions, network);
    }

    private static IEnumerable<T> Repeat<T>(IEnumerable<T> enumerable)
    {
        while (true)
        {
            foreach (T item in enumerable)
            {
                yield return item;
            }
        }
    }

    private static long LeastCommonMultiple(long a, long b) // lcm
    {
        return (a / GreatestCommonDivisor(a, b)) * b;
    }

    private static long GreatestCommonDivisor(long a, long b) // gcd
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        
        return a;
    }
}