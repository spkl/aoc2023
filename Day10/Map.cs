internal class Map(string[] rows)
{
    private readonly string[] rows = rows;

    private char this[Position p]
    {
        get
        {
            if (p.Y < 0 || p.X < 0 || p.Y > this.rows.Length - 1 || p.X > this.rows[p.Y].Length - 1)
            {
                return '.';
            }

            return this.rows[p.Y][p.X];
        }
    }

    internal Position FindStart()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            int x = rows[y].IndexOf('S');
            if (x != -1)
            {
                return new Position(x, y);
            }
        }

        throw new Exception("Could not find start in map.");
    }

    internal int GetStepsToFarthestPointFrom(Position start)
    {
        int steps = 1;
        (Position fwdPos, Position lastFwdPos) = this.Walk(start);
        (Position bwdPos, Position lastBwdPos) = this.Walk(start, fwdPos);
        
        do
        {
            (fwdPos, lastFwdPos) = this.Walk(fwdPos, lastFwdPos);
            (bwdPos, lastBwdPos) = this.Walk(bwdPos, lastBwdPos);
            
            steps++;
        } while (fwdPos != bwdPos);

        return steps;
    }

    private (Position toPos, Position fromPos) Walk(Position fromPos, params Position[] notPos)
    {
        List<Position> possiblePositions = [];
        switch (this[fromPos])
        {
            case 'S': // starting position of the animal - connects to exactly two pipes
                if (this[fromPos.LeftNeighbor] is '-' or 'L' or 'F')
                {
                    possiblePositions.Add(fromPos.LeftNeighbor);
                }

                if (this[fromPos.RightNeighbor] is '-' or 'J' or '7')
                {
                    possiblePositions.Add(fromPos.RightNeighbor);
                }

                if (this[fromPos.UpNeighbor] is '|' or '7' or 'F')
                {
                    possiblePositions.Add(fromPos.UpNeighbor);
                }

                if (this[fromPos.DownNeighbor] is '|' or 'L' or 'J')
                {
                    possiblePositions.Add(fromPos.DownNeighbor);
                }

                break;
            case '|': // vertical pipe connecting north and south (up and down)
                possiblePositions.Add(fromPos.UpNeighbor);
                possiblePositions.Add(fromPos.DownNeighbor);
                break;
            case '-': // horizontal pipe connecting east and west (right and left)
                possiblePositions.Add(fromPos.RightNeighbor);
                possiblePositions.Add(fromPos.LeftNeighbor);
                break;
            case 'L': // 90-degree bend connecting north and east (up and right)
                possiblePositions.Add(fromPos.UpNeighbor);
                possiblePositions.Add(fromPos.RightNeighbor);
                break;
            case 'J': // 90-degree bend connecting north and west (up and left)
                possiblePositions.Add(fromPos.UpNeighbor);
                possiblePositions.Add(fromPos.LeftNeighbor);
                break;
            case '7': // 90-degree bend connecting south and west (down and left)
                possiblePositions.Add(fromPos.DownNeighbor);
                possiblePositions.Add(fromPos.LeftNeighbor);
                break;
            case 'F': // is a 90-degree bend connecting south and east (down and right)
                possiblePositions.Add(fromPos.DownNeighbor);
                possiblePositions.Add(fromPos.RightNeighbor);
                break;
            default:
                throw new Exception("Unexpected character");
        }

        Position toPos = possiblePositions.Where(pos => !notPos.Contains(pos)).First();
        return (toPos, fromPos);
    }
}
