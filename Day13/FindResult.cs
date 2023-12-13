internal record struct FindResult(int Column, int Row)
{
    public static FindResult InColumn(int column) => new(column, -1);

    public static FindResult InRow(int row) => new(-1, row);

    public static FindResult None() => new(-1, -1);

    public readonly bool Success => this.Column != -1 || this.Row != -1;

    public readonly void AddToSum(ref int sum)
    {
        if (this.Column != -1)
        {
            sum += this.Column;
        }
        else if (this.Row != -1)
        {
            sum += 100 * this.Row;
        }
    }
}
