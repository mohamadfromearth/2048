public struct Vector2Int
{
    public int Y { get; set; }
    public int X { get; set; }

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }
}