using UnityEngine;

public class GameRepository : IGameRepository
{
    private int[,] _values;


    public void InitBoard(Vector2Int size)
    {
        Debug.Log("InitBoard");
        _values = new int[size.X, size.Y];
    }


    public void AddValue(Vector2Int pos, int value)
    {
        _values[pos.X, pos.Y] += value;
    }

    public int GetValue(Vector2Int pos)
    {
        return _values[pos.X, pos.Y];
    }

    public void SetValue(Vector2Int pos, int value)
    {
        _values[pos.X, pos.Y] = value;
    }

    public Vector2Int GetBoardSize() => new Vector2Int(_values.GetLength(0), _values.GetLength(1));
}