internal class Program
{
    private static void Main(string[] args)
    {
        string[] rows = File.ReadAllLines(@"..\..\..\input.txt");
        
        Map map = new(rows);
        Position start = map.FindStart();
        map.ReplaceStartSymbol(start);
        int steps = map.GetStepsToFarthestPointFrom(start);
        int enclosedPoints = map.GetEnclosedPoints();
        
        Console.WriteLine($"Steps: {steps}");
        Console.WriteLine($"Enclosed points: {enclosedPoints}");
    }
}