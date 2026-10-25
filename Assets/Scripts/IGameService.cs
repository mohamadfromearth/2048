using DefaultNamespace;

public interface IGameService
{
    public void InitBoard(Vector2Int size);

    public bool Move(MoveDirection direction);
    
    public int GetValue(Vector2Int position);

    public void SpawnNewValue();
        
}