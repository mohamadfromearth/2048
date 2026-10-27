using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "So/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public int boardWidth;
        public int boardHeight;
        public int targetScore;


        public Vector2Int BoardSize => new Vector2Int(boardWidth, boardHeight);

        public int TargetScore => targetScore;
    }
}