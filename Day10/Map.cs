internal class Map(string[] rows)
{
    private readonly string[] rows = rows;

    private readonly HashSet<Position> pathPositions = [];

    private char this[Position p]
    {
        get => this.IsOutsideMap(p) ? '.' : this.rows[p.Y][p.X];
        set
        {
            char[] row = this.rows[p.Y].ToCharArray();
            row[p.X] = value;
            this.rows[p.Y] = new string(row);
        }
    }

    internal Position FindStart()
    {
        for (int y = 0; y < this.rows.Length; y++)
        {
            int x = this.rows[y].IndexOf('S');
            if (x != -1)
            {
                Position start = new(x, y);
                this.pathPositions.Add(start);
                return start;
            }
        }

        throw new Exception("Could not find start in map.");
    }

    internal void ReplaceStartSymbol(Position start)
    {
        bool left = this.CanGoLeft(start);
        bool right = this.CanGoRight(start);
        bool up = this.CanGoUp(start);
        bool down = this.CanGoDown(start);

        char newSymbol = new bool[]{ left, right, up, down } switch {
            [true, true, false, false] => '-',
            [true, false, true, false] => 'J',
            [true, false, false, true] => '7',
            [false, true, true, false] => 'L',
            [false, true, false, true] => 'F',
            [false, false, true, true] => '|',
            _ => throw new Exception("Start does not connect to exactly two points.")
        };

        this[start] = newSymbol;
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

    internal int GetEnclosedPoints()
    {
        Position upLeftLimit = new(this.pathPositions.Select(p => p.X).Min(), this.pathPositions.Select(p => p.Y).Min());
        Position downRightLimit = new(this.pathPositions.Select(p => p.X).Max(), this.pathPositions.Select(p => p.Y).Max());
        HashSet<Position> enclosedPoints = [];

        foreach (Position p in this.GetPointsInSpan(upLeftLimit, downRightLimit))
        {
            if (this.pathPositions.Contains(p) 
                || !this.IsEnclosedInPath(p, upLeftLimit))
            {
                continue;
            }

            enclosedPoints.Add(p);
        }

        return enclosedPoints.Count;
    }

    private (Position toPos, Position fromPos) Walk(Position fromPos, params Position[] notPos)
    {
        List<Position> possiblePositions = [];
        switch (this[fromPos])
        {
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
        this.pathPositions.Add(toPos);
        return (toPos, fromPos);
    }

    private bool IsEnclosedInPath(Position p, Position upLeftLimit)
    {
        // Idea: If the number of path-crossings to one side is odd, the point is inside the path.
        // Approach:
        // Go straight left from the point p to the leftmost limit of the span.
        // When encountering a path node:
        // - If the path node is in our direction of travel (-), it does not influence the result.
        // - If the path node is perpendicular to our direction of travel (|), it counts as as a crossing.
        // - Every pair of up (J or L) and down (7 or F) elbow nodes counts as one additional crossing.
        //   (=> Something like 'F--7' does not count as a crossing, but 'L--7' does.)

        int crossings = 0;
        int upElbows = 0;
        int downElbows = 0;

        for (int x = p.X - 1; x >= upLeftLimit.X; x--)
        {
            Position test = new(x, p.Y);
            if (!this.pathPositions.Contains(test))
            {
                continue;
            }

            switch (this[test])
            {
                case '-':
                    break;
                case '|':
                    crossings++;
                    break;
                case 'J':
                case 'L':
                    upElbows++;
                    break;
                case '7':
                case 'F':
                    downElbows++;
                    break;
            }
        }

        while (upElbows > 0 && downElbows > 0)
        {
            crossings++;
            upElbows--;
            downElbows--;
        }

        return crossings % 2 != 0;
    }

    private bool IsOutsideMap(Position p) => p.Y < 0 || p.X < 0 || p.Y > this.rows.Length - 1 || p.X > this.rows[p.Y].Length - 1;

    private bool CanGoLeft(Position pos) => this[pos.LeftNeighbor] is '-' or 'L' or 'F';

    private bool CanGoRight(Position pos) => this[pos.RightNeighbor] is '-' or 'J' or '7';

    private bool CanGoUp(Position pos) => this[pos.UpNeighbor] is '|' or '7' or 'F';

    private bool CanGoDown(Position pos) => this[pos.DownNeighbor] is '|' or 'L' or 'J';

    private IEnumerable<Position> GetPointsInSpan(Position upLeftLimit, Position downRightLimit)
    {
        for (int x = upLeftLimit.X; x <= downRightLimit.X; x++)
        {
            for (int y = upLeftLimit.Y; y <= downRightLimit.Y; y++)
            {
                Position p = new(x, y);
                yield return p;
            }
        }
    }
}
