using UnityEngine;

namespace DefaultNamespace
{
    public interface IGameService
    {
        public void InitBoard(Vector2Int size);

        public bool Move(MoveDirection direction);
    }
}