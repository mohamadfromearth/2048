using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "So/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField]private int boardWidth = 4;
        [SerializeField]private int boardHeight = 4;
        [SerializeField]private int targetScore = 2048;


        public Vector2Int BoardSize => new Vector2Int(boardWidth, boardHeight);

        public int TargetScore => targetScore;
    }
}