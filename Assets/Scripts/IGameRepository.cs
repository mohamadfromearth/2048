public interface IGameRepository
{
    public void InitBoard(Vector2Int size);

    public void AddValue(Vector2Int pos, int value);

    public int GetValue(Vector2Int pos);

    public void SetValue(Vector2Int pos, int value);

    public Vector2Int GetBoardSize();

}