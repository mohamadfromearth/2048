using System;
using System.Collections.Generic;

public interface IGameService
{
    public event Action<MoveData> OnMove;
    public event Action<MoveData> OnMerge;


    public void InitBoard(Vector2Int size);

    public bool Move(MoveDirection direction);

    public bool CanMove();

    public int GetValue(Vector2Int position);

    public List<SpawnData> SpawnNewValues(int count);
    
    public int GetScore();
}