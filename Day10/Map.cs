internal class Map(string[] rows)
{
    private readonly string[] rows = rows;

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
                return new Position(x, y);
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
        return (toPos, fromPos);
    }

    private bool IsOutsideMap(Position p) => p.Y < 0 || p.X < 0 || p.Y > this.rows.Length - 1 || p.X > this.rows[p.Y].Length - 1;

    private bool CanGoLeft(Position pos) => this[pos.LeftNeighbor] is '-' or 'L' or 'F';

    private bool CanGoRight(Position pos) => this[pos.RightNeighbor] is '-' or 'J' or '7';

    private bool CanGoUp(Position pos) => this[pos.UpNeighbor] is '|' or '7' or 'F';

    private bool CanGoDown(Position pos) => this[pos.DownNeighbor] is '|' or 'L' or 'J';
}
