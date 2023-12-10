﻿internal class Program
{
    private static void Main(string[] args)
    {
        string[] rows = File.ReadAllLines(@"..\..\..\input.txt");
        Map map = new(rows);
        Position start = map.FindStart();
        map.ReplaceStartSymbol(start);
        Console.WriteLine(map.GetStepsToFarthestPointFrom(start));
    }
}